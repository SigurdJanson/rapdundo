using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;


namespace RapdUnDo.IUndoCore
{
    public class FleximandService : IFleximandService
    {
        /// <summary>
        /// The snackbar service needed to display the undo feature to the users.
        /// </summary>
        protected ISnackbar SnackbarService { get; set; } // injection

        /// <summary>
        /// The list of commands managed by this service.
        /// </summary>
        protected HashSet<IFleximand> Commands { get; set; } = new();


        /// <inheritdoc/>
        public event EventHandler NotifyPageOnCommand;

        /// <inheritdoc/>
        public int GracePeriod { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="snackbar">Injected ISnackbar service</param>
        /// <param name="gracePeriod">Sets <see cref="GracePeriod"/></param> //<include path='[@name="GracePeriod"]' />
        public FleximandService(ISnackbar snackbar, int gracePeriod = 5000)
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
                c.Revoked -= OnRevoked;
            }
            GC.SuppressFinalize(this);
        }



        /// <inheritdoc/>
        public void Register(IFleximand command)
        {
            Commands.Add(command);
            command.Executed += OnExecuted;
            command.Revoked += OnRevoked;
        }


        /// <inheritdoc/>
        public void RegisterList(IEnumerable<IFleximand> commands)
        {
            foreach (var c in commands)
                Register(c);
        }



        /// <inheritdoc/>
        public void OnExecuted(object command, FmdExecEventArgs eventArgs)
        {
            if ((command as IFleximand)?.CanRevoke(eventArgs.CommandParameter) ?? false)
            {
                Show(command as IFleximand, eventArgs.CommandParameter);
            }
        }


        /// <inheritdoc/>
        public void OnRevoked(object command, FmdExecEventArgs eventArgs)
        {
            Show(command as IFleximand, eventArgs.CommandParameter);
        }



#nullable enable
        protected void Show(IFleximand command, object? commandParameter)
        {
            //TODO: l10n
            SnackbarService.Add($"{command.Name}: {command.ExecutionMessage}", Severity.Normal, config =>
            {
                if ((command as IFleximand)?.CanRevoke(commandParameter) ?? false) 
                    config.Action = "Undo"; //TODO: l10n
                config.ActionColor = Color.Primary;
                config.Onclick = snackbar =>
                {
                    command.Revoke(commandParameter);

                    return Task.CompletedTask;
                };
            });
 
            NotifyPageOnCommand?.Invoke(this, EventArgs.Empty);
        }
#nullable restore
    }
}