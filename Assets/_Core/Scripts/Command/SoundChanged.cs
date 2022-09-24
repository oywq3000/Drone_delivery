using _Core.Drove.Event;
using QFramework;

namespace Drove.Command.Command
{
    public class SoundChanged : AbstractCommand
    {
        private float _valuem;

        public SoundChanged(float valuem)
        {
            _valuem = valuem;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new OnSoundVolumeChanged() {Volume = _valuem});
        }
    }
}