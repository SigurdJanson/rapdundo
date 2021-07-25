using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;

namespace RapdUnDo.IUndoCore
{
    public interface IUndoableService : IDisposable
    {
        /// <summary>
        /// The time period a UI widget is visible and allows users to undo an action.
        /// </summary>
        public int GracePeriod { get; set; }

        public void Register(IUndoableCommand command);

        public void RegisterList(IEnumerable<IUndoableCommand> commands);

        public void OnExecuted(IUndoableCommand command);
    }
}
