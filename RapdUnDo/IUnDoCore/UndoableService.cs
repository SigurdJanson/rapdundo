using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;


namespace RapdUnDo.IUndoCore
{
    public class UndoableService : IUndoableService
    {
        /// <summary>
        /// The snackbar service needed to display the undo feature to the users.
        /// </summary>
        protected ISnackbar SnackbarService { get; set; } // injection

        /// <summary>
        /// The list of commands managed by this service.
        /// </summary>
        protected HashSet<IUndoableCommand> Commands { get; set; } = new();


        /// <inheritdoc/>
        public int GracePeriod { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="snackbar">Injected ISnackbar service</param>
        /// <param name="gracePeriod">Sets <see cref="GracePeriod"/></param> //<include path='[@name="GracePeriod"]' />
        public UndoableService(ISnackbar snackbar, int gracePeriod = 5000)
        {
            SnackbarService = snackbar;
            GracePeriod = gracePeriod;
        }



        /// <inheritdoc/>
        /// <remarks>Dispose method [...] should be callable multiple times without throwing an exception.</remarks>
        void IDisposable.Dispose()
        {
            foreach (var c in Commands)
            {
                c.Executed -= OnExecuted;
                //c.Revoked -= OnRevoked; // currently not needed
            }
            GC.SuppressFinalize(this);
        }



        /// <inheritdoc/>
        public void Register(IUndoableCommand command)
        {
            Commands.Add(command);
            command.Executed += OnExecuted;
        }

        
        /// <inheritdoc/>
        public void RegisterList(IEnumerable<IUndoableCommand> commands)
        {
            foreach (var c in commands)
                Register(c);
        }



        /// <inheritdoc/>
        public void OnExecuted(object command, CmdExecEventArgs eventArgs)
        {
            if ((command as IUndoableCommand)?.CanRevoke(eventArgs.CommandParameter) ?? false)
            {
                Show(command as IUndoableCommand, eventArgs.CommandParameter);
            }
        }


        /// <inheritdoc/>
        public void OnRevoked(object command, CmdExecEventArgs eventArgs)
        {
            if ((command as IUndoableCommand)?.CanRevoke(eventArgs.CommandParameter) ?? false)
            {
                Show(command as IUndoableCommand, eventArgs.CommandParameter);
            }
        }



#nullable enable
        protected void Show(IUndoableCommand command, object? commandParameter)
        {
            //TODO: l10n
            SnackbarService.Add($"{command.CommandName}: {command.ExecutionMessage}", Severity.Normal, config =>
            {
                config.Action = "Undo"; //TODO: l10n
                config.ActionColor = Color.Primary;
                config.Onclick = snackbar =>
                {
                    command.Revoke(commandParameter);
                    return Task.CompletedTask;
                };
            });
        }
#nullable restore
    }
}