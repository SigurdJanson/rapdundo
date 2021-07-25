using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;


namespace RapdUnDo.IUndoCore
{
    public class UndoableService : IUndoableService
    {
        protected ISnackbar SnackbarService { get; set; }

        protected HashSet<IUndoableCommand> Commands { get; set; }

        public int GracePeriod { get; set; }

        public UndoableService(ISnackbar snackbar, int gracePeriod = 5000)
        {
            SnackbarService = snackbar;
            GracePeriod = gracePeriod;
        }

        public void OnExecuted(IUndoableCommand command, object? commandParameter)
        {
            if (command?.CanRevoke(null) ?? false)
            {
                Show(command, commandParameter);
            }
        }


        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
            GC.SuppressFinalize(this);
        }


        public void Register(IUndoableCommand command)
        {
            Commands.Add(command);
            command.Executed += OnExecuted;
            throw new NotImplementedException(); // register to fire event
        }

        public void RegisterList(IEnumerable<IUndoableCommand> commands)
        {
            foreach (var c in commands)
                Register(c);
        }


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
    }
}