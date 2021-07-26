using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapdUnDo.IUndoCore
{
    public abstract class UndoableCommandBase : IUndoableCommand
    {
        /// <inheritdoc/>
        public string CommandName { get; set; } = "Changed Property";

        /// <inheritdoc/>
        public string ExecutionMessage { get; set; } = "Command has been executed";


#nullable enable
        protected void NotifyExecution(object? parameter) =>
            Executed?.Invoke(this, new CmdExecEventArgs()
            {
                CommandName = this.CommandName,
                Message = this.ExecutionMessage,
                CommandParameter = parameter
            });

        protected void NotifyRevocation(object? parameter) =>
            Revoked?.Invoke(this, new CmdExecEventArgs()
            {
                CommandName = this.CommandName,
                Message = this.ExecutionMessage,
                CommandParameter = parameter
            });
#nullable restore


#nullable enable
        /// <inheritdoc/>
        abstract public void Execute(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler<CmdExecEventArgs>? Executed;

        /// <inheritdoc/>
        abstract public bool CanExecute(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Invoke the <c>CanExecuteChanged</c> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyOnCanExecuteChanged(object sender, CmdExecEventArgs e) => 
            CanExecuteChanged?.Invoke(sender, e);

        /// <inheritdoc/>
        abstract public void Revoke(object? parameter);

        /// <inheritdoc/>
        public event EventHandler<CmdExecEventArgs>? Revoked;

        /// <inheritdoc/>
        abstract public bool CanRevoke(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler? CanRevokeChanged;

        /// <summary>
        /// Invoke the <c>CanRevokeChanged</c> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyOnCanRevokeChanged(object sender, CmdExecEventArgs e) =>
            CanExecuteChanged?.Invoke(sender, e);
#nullable restore

        abstract public RenderFragment DisplayCommand();
    }
}
