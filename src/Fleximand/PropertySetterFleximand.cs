using Microsoft.AspNetCore.Components;
using System;

namespace Fleximand.Core
{

    /// <summary>
    /// Generic fleximand class that allows setting properties and undo this. You can execute
    /// the command with one <c>parameter</c> and revoke it with the exact same <c>parameter</c>.
    /// It cannot be applied several times.
    /// </summary>
    /// <typeparam name="TO">Type of the object</typeparam>
    /// <typeparam name="TV">Type of the object's property</typeparam>
    public class PropertySetterFleximand<TO, TV> : FleximandBase where TO : class where TV : IConvertible
    {
        public int ExecutionTimes { get; protected set; } = 0;

        /// <summary>Value before executing the command</summary>
        public TV OldValue { get; protected set; }

        /// <summary>The object reference</summary>
        public TO Ref { get; protected set; }

        /// <summary>The name of the property that is set</summary>
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
        /// <param name="_PropertyName">
        /// The name of the property that is to be changed. Use <c>nameof(...)</c> to set this parameter.
        /// The property must be public.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public PropertySetterFleximand(TO _Object, string _PropertyName)
        {
            if (_Object is null) throw new ArgumentNullException(nameof(_Object));
            if (_Object.GetType().GetProperty(_PropertyName) == null)
                throw new ArgumentException($"Property '{_PropertyName}' does not exist " +
                    $"in class {_Object.GetType()}", nameof(_PropertyName));

            PropertyName = _PropertyName;
            this.Ref = _Object; // keep the reference
            OldValue = Property;
        }



#nullable enable
        /// <inheritdoc/>
        public override void Execute(object? parameter)
        {
            Property = parameter is null ? default : (TV)parameter;
            if (!CanRevoke(parameter)) NotifyOnCanRevokeChanged(this, new FmdExecEventArgs());
            ExecutionTimes++;
            NotifyExecution(parameter);
        }

        /// <inheritdoc/>
        public override bool CanExecute(object? parameter) 
            => !Property.Equals(parameter);


        /// <inheritdoc/>
        public override void Revoke(object? parameter)
        {
            if (CanRevoke(parameter))
            {
                Property = OldValue;
                ExecutionTimes--;
                NotifyRevocation(parameter);
                if (!CanRevoke(parameter)) NotifyOnCanRevokeChanged(this, new FmdExecEventArgs());
            }
        }

        /// <inheritdoc/>
        public override bool CanRevoke(object? parameter) 
            => ExecutionTimes > 0 && Property.Equals(parameter);

#nullable restore


        /// <inheritdoc/>
        public override RenderFragment Display() 
        {
            return b => // b is a RenderTreeBuilder
            {
                int i = 0;
                b.OpenElement(i++, "div");
                b.OpenElement(i++, "p");
                b.AddContent(i++, $"Old value: {OldValue}");
                b.CloseElement();
                b.OpenElement(i++, "p");
                b.AddContent(i++, $"New value: "); // PROBLEM
                b.CloseElement();
                b.CloseElement();
            };
        }
    }

}