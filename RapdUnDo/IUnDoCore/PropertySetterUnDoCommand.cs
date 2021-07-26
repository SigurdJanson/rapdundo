using Microsoft.AspNetCore.Components;
using System;

namespace RapdUnDo.IUndoCore
{

    /// <summary>
    /// Generic undoable command class that allows setting properties and undo this.
    /// </summary>
    /// <typeparam name="TO">Type of the object</typeparam>
    /// <typeparam name="TV">Type of the object's property</typeparam>
    public class PropertySetterUndoCommand<TO, TV> : UndoableCommandBase where TO : class where TV : IConvertible
    {
        public int ExecutionTimes { get; protected set; } = 0;

        /// <summary>Value before executing the command</summary>
        public TV OldValue { get; protected set; }
        /// <summary>Value AFTER executing the command</summary>
        public TV NewValue { get; protected set; }

        /// <summary>The object reference</summary>
        public TO Ref { get; protected set; }

        /// <summary>The name</summary>
        public string PropertyName { get; protected set; }

        /// <summary>Direct access to the property <c>PropertyName</c> of object <c>Ref</c></summary>
        protected TV Property
        {
            get => (TV)Ref.GetType().GetProperty(PropertyName).GetValue(Ref);
            set => Ref.GetType().GetProperty(PropertyName).SetValue(Ref, value);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_Object">The reference object that is going to be modified by the command.</param>
        /// <param name="_NewValue">The value after executing the command</param>
        /// <param name="_PropertyName">
        /// The name of the property that is to be changed. Use <c>nameof(...)</c> to set this parameter.
        /// The property must be public.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public PropertySetterUndoCommand(TO _Object, string _PropertyName, TV _NewValue)
        {
            if (_Object is null) throw new ArgumentNullException(nameof(_Object));
            if (_Object.GetType().GetProperty(_PropertyName) == null)
                throw new ArgumentException($"Property '{_PropertyName}' does not exist in class {_Object.GetType()}", nameof(_PropertyName));

            PropertyName = _PropertyName;
            this.Ref = _Object; // keep the reference
            OldValue = Property;
            NewValue = _NewValue;
        }



#nullable enable
        /// <inheritdoc/>
        public override void Execute(object? parameter = null)
        {
            Property = NewValue;
            if (ExecutionTimes == 0) NotifyOnCanRevokeChanged(this, new CmdExecEventArgs());
            ExecutionTimes++;
            NotifyExecution(parameter);
        }

        /// <inheritdoc/>
        public override bool CanExecute(object? parameter = null) => true;


        /// <inheritdoc/>
        public override void Revoke(object? parameter = null)
        {
            if (ExecutionTimes <= 0) 
                throw new Exception("Command cannot be executed");

            Property = OldValue;
            ExecutionTimes--;
            NotifyRevocation(parameter);
        }

        /// <inheritdoc/>
        public override bool CanRevoke(object? parameter = null) => ExecutionTimes > 0;

#nullable restore


        /// <inheritdoc/>
        public override RenderFragment DisplayCommand() 
        {
            return b => // b is a RenderTreeBuilder
            {
                int i = 0;
                b.OpenElement(i++, "div");
                b.OpenElement(i++, "p");
                b.AddContent(i++, $"Old value: {OldValue}");
                b.CloseElement();
                b.OpenElement(i++, "p");
                b.AddContent(i++, $"New value: {NewValue}");
                b.CloseElement();
                b.CloseElement();
            };
        }
    }

}