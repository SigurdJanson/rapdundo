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

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
            GC.SuppressFinalize(this);
        }


        public void Register(IUndoableCommand command)
        {
            Commands.Add(command);
            throw new NotImplementedException(); // register to fire event
        }

        public void RegisterList(IEnumerable<IUndoableCommand> commands)
        {
            foreach (var c in commands)
                Register(c);
        }


        protected void Show()
        {
            SnackbarService.Add("Ooops. Something really bad happened!", Severity.Normal, config =>
            {
                config.Action = "Help";
                config.ActionColor = Color.Primary;
                config.Onclick = snackbar =>
                {
                    Help();
                    return Task.CompletedTask;
                };
            });
        }
    }
}