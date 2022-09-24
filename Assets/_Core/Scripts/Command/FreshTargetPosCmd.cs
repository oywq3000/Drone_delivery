
using QFramework;
namespace Drove.Command.Command
{
    public class FreshTargetPosCmd:AbstractCommand
    {
        //Indicates 
        public bool isFreshed;
        protected override void OnExecute()
        {
            var droneDataMod = this.GetModel<PressedType>();
        }
    }
}