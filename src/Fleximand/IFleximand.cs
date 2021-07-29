using System;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;


namespace Fleximand.Core
{
#nullable enable

    /// <summary>
    /// Defines an interface for command that can be undone in Blazor Apps.
    /// </summary>
    public interface IFleximand : ICommand
    {
        /// <summary>
        /// Callback mechanism to inform command handlers that the command has been executed.
        /// </summary>
        public event EventHandler<FmdExecEventArgs>? Executed;

        /// <summary>
        /// Callback mechanism to inform command handlers that the command has been revoked.
        /// </summary>
        public event EventHandler<FmdExecEventArgs>? Revoked;


        /// <summary>
        /// Defines the method to be called when the <em>undo</em> command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to <c>null</c>.</param>
        public void Revoke(object? parameter);

        /// <summary>
        /// Defines the method that determines whether the command can revert in its current state.
        /// </summary>
        /// <remarks>That implies that the command has executed before.</remarks>
        /// <param name="parameter">Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to <c>null</c>.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanRevoke(object? parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should revert.
        /// </summary>
        /// <remarks>Normally, a command source calls <see cref="CanRevoke">CanRevert</see> on the command when this event occurs.</remarks>
        public event EventHandler? CanRevokeChanged;


        /// <summary>
        /// A self-descriptive string that identifies the command to the user.
        /// </summary>
        /// <remarks>Usually identical to the name of the button, menu item, etc. that elicits the commands</remarks>
        public string Name { get; set; }

        /// <summary>
        /// A self-descriptive message shown to the users after the command has been executed.
        /// </summary>
        public string ExecutionMessage { get; set; }

        /// <summary>
        /// Return a piece of a Blazor render fragment that can be displayed in a window
        /// to explain users what happens for them decide whether they want to undo or not.
        /// </summary>
        /// <returns>A (hopefully) user-friendly display of the command's effects.</returns>
        public RenderFragment Display();
    }

#nullable restore
}
