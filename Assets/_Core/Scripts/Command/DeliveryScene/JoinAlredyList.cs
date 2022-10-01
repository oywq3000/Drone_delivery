using _Core.Drove.Event;
using _Core.Drove.Script.Interface;
using QFramework;

namespace Drove.Command.Command
{
    public class JoinAlredyList : AbstractCommand
    {
        //when a drone has already to diverse send the command
        private IAutoDrone _autoController;

        public JoinAlredyList(IAutoDrone autoController)
        {
            _autoController = autoController;
        }
        protected override void OnExecute()
        {
            if (_autoController!=null)
            {
                this.SendEvent(new ReigsterMySelf(){DroneControllor = _autoController});
            }
        }
    }
}