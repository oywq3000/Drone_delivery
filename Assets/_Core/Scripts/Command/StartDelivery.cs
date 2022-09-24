
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class StartDelivery: AbstractCommand
    {
        protected override void OnExecute()
        {
          this.SendEvent<DroneStartDelivery>();
        }
    }
}