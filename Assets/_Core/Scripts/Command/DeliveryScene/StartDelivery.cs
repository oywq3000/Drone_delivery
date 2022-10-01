
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using QFramework;
namespace Drove.Command.Command
{
    public class StartDelivery: AbstractCommand
    {
        protected override void OnExecute()
        {
            //Trigger Drone Start to delivery
            this.SendEvent<DroneStartDelivery>();
        }
    }
}