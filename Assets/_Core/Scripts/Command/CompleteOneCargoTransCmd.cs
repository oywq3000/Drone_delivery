
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class CompleteOneCargoTransCmd: AbstractCommand
    {
        protected override void OnExecute()
        {
            //trigger the Event that 
            this.SendEvent<OnOneCargoHasSent>();
        }
    }
}