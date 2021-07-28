using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapdUnDo.IUndoCore
{
    public abstract class FleximandBase : IFleximand
    {
        /// <inheritdoc/>
        public string Name { get; set; } = "Changed Property";

        /// <inheritdoc/>
        public string ExecutionMessage { get; set; } = "Command has been executed";


#nullable enable
        protected void NotifyExecution(object? parameter) =>
            Executed?.Invoke(this, new FmdExecEventArgs()
            {
                CommandName = this.Name,
                Message = this.ExecutionMessage,
                CommandParameter = parameter
            });

        protected void NotifyRevocation(object? parameter) =>
            Revoked?.Invoke(this, new FmdExecEventArgs()
            {
                CommandName = this.Name,
                Message = this.ExecutionMessage,
                CommandParameter = parameter
            });
#nullable restore


#nullable enable
        /// <inheritdoc/>
        abstract public void Execute(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler<FmdExecEventArgs>? Executed;

        /// <inheritdoc/>
        abstract public bool CanExecute(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Invoke the <c>CanExecuteChanged</c> event.
        /// </summary>
        /// <param name="sender">The command</param>
        /// <param name="e">The command's data</param>
        protected void NotifyOnCanExecuteChanged(object sender, FmdExecEventArgs e) => 
            CanExecuteChanged?.Invoke(sender, e);

        /// <inheritdoc/>
        abstract public void Revoke(object? parameter);

        /// <inheritdoc/>
        public event EventHandler<FmdExecEventArgs>? Revoked;

        /// <inheritdoc/>
        abstract public bool CanRevoke(object? parameter = null);

        /// <inheritdoc/>
        public event EventHandler? CanRevokeChanged;

        /// <summary>
        /// Invoke the <c>CanRevokeChanged</c> event.
        /// </summary>
        /// <param name="sender">The command</param>
        /// <param name="e">The command's data</param>
        protected void NotifyOnCanRevokeChanged(object sender, FmdExecEventArgs e) =>
            CanRevokeChanged?.Invoke(sender, e);
#nullable restore

        /// <inheritdoc/>
        abstract public RenderFragment Display();
    }
}
