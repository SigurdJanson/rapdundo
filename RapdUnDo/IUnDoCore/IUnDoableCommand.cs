using System;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;


namespace RapdUnDo.IUndoCore
{
#nullable enable

    /// <summary>
    /// Defines an interface for command that can be undone in Blazor Apps.
    /// </summary>
    interface IUnDoableCommand : ICommand
    {
        /// <summary>
        /// Defines the method to be called when the <em>undo</em> command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to <c>null</c>.</param>
        public void Revert(object? parameter);

        /// <summary>
        /// Defines the method that determines whether the command can revert in its current state.
        /// </summary>
        /// <remarks>That implies that the command has executed before.</remarks>
        /// <param name="parameter">Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to <c>null</c>.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanRevert(object? parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should revert.
        /// </summary>
        /// <remarks>Normally, a command source calls <see cref="CanRevert">CanRevert</see> on the command when this event occurs.</remarks>
        public event EventHandler? CanRevertChanged;


        /// <summary>
        /// Return a piece of a Blazor render fragment that can be displayed in a window
        /// to explain users what happens for them decide whether they want to undo or not.
        /// </summary>
        /// <returns>A (hopefully) user-friendly display of the command's effects.</returns>
        public RenderFragment DisplayCommand();
    }

#nullable restore
}
