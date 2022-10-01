
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class TriggerOnGetEvent: AbstractCommand
    {
        private readonly PressedKeyCode[] _pressedKeyCodes;
        public TriggerOnGetEvent(PressedKeyCode[] pressedKeyCodes)
        {
            _pressedKeyCodes = pressedKeyCodes;
        }
        protected override void OnExecute()
        {
            this.SendEvent(new OnGetEventPressed(){PressedKeyCodes = _pressedKeyCodes});
        }
    }
}