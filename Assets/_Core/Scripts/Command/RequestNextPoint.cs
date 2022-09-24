
using _Core.Drove.Event;
using _Core.Drove.Script.Interface;
using QFramework;
namespace Drove.Command.Command
{
    public class RequestNextPoint: AbstractCommand
    {
        //when a drone has already to diverse send the command
        private IAutoDrone _autoController;
        public RequestNextPoint(IAutoDrone autoController)
        {
            _autoController = autoController;
        }
        //this command indicates this drone has completed before instruction
        protected override void OnExecute()
        {
            
        }
    }
}