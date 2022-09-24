
using _Core.Drove.Event;
using QFramework;
namespace Drove.Command.Command
{
    public class InitiateFlightChannel: AbstractCommand
    {
        private int _cannelCount;
        //clearance of adjacent channel
        private float _cannelSpecing;
        //the original height of channel
        private float _originalHeight;
        public InitiateFlightChannel(int cannelCount, float cannelSpecing, float originalHeight)
        {
            _cannelCount = cannelCount;
            _cannelSpecing = cannelSpecing;
            _originalHeight = originalHeight;
        }
        protected override void OnExecute()
        {
            var channelList = this.GetModel<IAerialDroneCount>().ChannelList;
            for (int i = 0; i < _cannelCount; i++)
            {
                channelList.Add(_originalHeight+_cannelSpecing*i);
            }
        }
    }
}