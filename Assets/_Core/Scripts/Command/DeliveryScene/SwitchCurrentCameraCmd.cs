
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class SwitchCurrentCameraCmd: AbstractCommand
    {
        protected override void OnExecute()
        {
            //trigger the Event that 
            this.SendEvent(new OnArrivedTargetPos());
        }
    }
}