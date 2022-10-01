using _Core.Drove.Event;
using QFramework;

namespace Drove.Command.Command
{
    public class DroneInfSwitchChanged : AbstractCommand
    {
        private bool _enable;

        public DroneInfSwitchChanged(bool enable)
        {
            _enable = enable;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new OnInfSwitchChanged() {enable = _enable});
        }
    }
}