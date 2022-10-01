
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class EnableLightTrick: AbstractCommand
    {
        private static bool isEnable = true;
        protected override void OnExecute()
        {
            //trigger the Event that 
            this.SendEvent(new EnableLigthNav(){IsEnable = isEnable});
            isEnable = !isEnable;
        }
    }
}