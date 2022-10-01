
using _Core.Drove.Event;
using QFramework;
using QFramework.Example;

namespace Drove.Command.Command
{
    public class InitiateDeliveryScene: AbstractCommand
    {
        protected override void OnExecute()
        {
            //Init Reskit tool
            ResKit.Init();
            //Set Channel Configuration
            this.GetModel<IAerialDroneCount>().InitChannel(30, 4, 5, 25);
            //Init completed then entry the first process: open EntryPanel
            UIKit.OpenPanel<EntryPanel>();
        }
    }
}