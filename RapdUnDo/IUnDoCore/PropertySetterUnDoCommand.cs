using Microsoft.AspNetCore.Components;
using System;

namespace RapdUnDo.IUndoCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class PropertySetterUnDoCommand<TO, TV> : IUnDoableCommand where TO : class where TV : IConvertible
    {
        public TV OldValue { get; protected set; }
        public TV NewValue { get; protected set; }

        public TO Ref { get; protected set; }

        public string PropertyName { get; protected set; }

        protected TV Property
        {
            get => (TV)Ref.GetType().GetProperty(PropertyName).GetValue(Ref, null);
            set => Ref.GetType().GetProperty(PropertyName).SetValue(Ref, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Object"></param>
        /// <param name="_NewValue"></param>
        /// <param name="_PropertyName"></param>
        public PropertySetterUnDoCommand(TO _Object, TV _NewValue, string _PropertyName)
        {
            if (_Object.GetType().GetProperty(_PropertyName) == null)
                throw new ArgumentException($"Property '{_PropertyName}' does not exist", nameof(_PropertyName));

            this.Ref = _Object; // keep the reference
            OldValue = (TV)Ref.GetType().GetProperty(PropertyName).GetValue(Ref, null); ;
            NewValue = _NewValue;
            PropertyName = _PropertyName;
        }

        /// <inheritdoc/>
        public void Execute()
        {
            Property = NewValue;
        }

        /// <inheritdoc/>
        public void Revert()
        {
            Property = OldValue;
        }

        /// <inheritdoc/>
        //public RenderFragment DisplayCommand() { return new UnDoCoreDefaultUI(); }
    }
}
