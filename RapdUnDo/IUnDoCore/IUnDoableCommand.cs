using Microsoft.AspNetCore.Components;


namespace RapdUnDo.IUndoCore
{
    /// <summary>
    /// Defines an interface for command that can be undone in Blazor Apps .
    /// </summary>
    interface IUnDoableCommand
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        public void Execute();

        /// <summary>
        /// Undo the command after it was executed.
        /// </summary>
        public void Revert();

        //public RenderFragment DisplayCommand();
    }
}
