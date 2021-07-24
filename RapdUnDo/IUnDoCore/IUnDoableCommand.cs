using Microsoft.AspNetCore.Components;


namespace RapdUnDo.IUndoCore
{
    interface IUnDoableCommand
    {
        public void Execute();

        public void Revert();

        //public RenderFragment DisplayCommand();
    }
}
