using System;
using System.Collections.Generic;


namespace Fleximand.Core
{
    public interface IFleximandService : IDisposable
    {
        /// <summary>
        /// The time period (milliseconds) that users get the chance to undo an action.
        /// </summary>
        public int GracePeriod { get; set; }

        /// <summary>
        /// Register a single command. Make sure to hook <c>OnExecute</c>/<c>OnRevoked</c> into the 
        /// callbacks <c>Executed</c> and <c>Revoked</c> to get notified of these events.
        /// </summary>
        /// <param name="command"></param>
        public void Register(IFleximand command);

        /// <summary>
        /// Register a list of commands
        /// </summary>
        /// <param name="commands"></param>
        public void RegisterList(IEnumerable<IFleximand> commands);


        public event EventHandler NotifyPageOnCommand;


        /// <summary>
        /// Get notified by an event handler that a command was executed.
        /// </summary>
        /// <param name="sender">The command (<see cref="IFleximand"></see>)</param>
        /// <param name="eventArgs">Command data</param>
        public void OnExecuted(object sender, FmdExecEventArgs eventArgs);


        /// <summary>
        /// Get notified by an event handler that a command was revoked.
        /// </summary>
        /// <param name="sender">The command (<see cref="IFleximand"></see>)</param>
        /// <param name="eventArgs">Command data</param>
        public void OnRevoked(object sender, FmdExecEventArgs eventArgs);
    }
}
