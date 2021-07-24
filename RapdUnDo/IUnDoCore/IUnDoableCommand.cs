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

        /// <summary>
        /// Return a piece of a Blazor render fragment that can be displayed in a window
        /// to explain users what happens for them decide whether they want to undo or not.
        /// </summary>
        /// <returns>A (hopefully) user-friendly display of the command's effects.</returns>
        public RenderFragment DisplayCommand();
    }
}
