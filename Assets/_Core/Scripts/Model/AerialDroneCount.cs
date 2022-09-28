using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using _Core.Scripts;
using Drove.Command.Command;
using UnityEngine;

namespace Drove
{
    public interface IAerialDroneCount : IModel
    {
        SortedDictionary<int, int> Channel { get; set; }
        int ChannelCount { get; }
        int ChannelSpan { get; }
        int ChannelVolume { get; }
        int OriginalHeight { get; }

        void InitChannel(int channelCount = 30, int channelSpan = 3, int channelVolume = 5,
            int originalHeight = 20);
    }

    public class AerialDroneCount : AbstractModel, IAerialDroneCount
    {
        public int ChannelCount { get; set; }
        public int ChannelSpan { get; set; }
        public int ChannelVolume { get; set; }
        public int OriginalHeight { get; set; }


        public SortedDictionary<int, int> Channel { get; set; }

        public void InitChannel(int channelCount = 30, int channelSpan = 3, int channelVolume = 5,
            int originalHeight = 20)
        {
            //init variable
            ChannelCount = channelCount;
            ChannelSpan = channelSpan;
            ChannelVolume = channelVolume;
            OriginalHeight = originalHeight;
            if (Channel == null)
            {
                Channel = new SortedDictionary<int, int>();
            }
            else
            {
                Channel.Clear();
            }

            for (int i = 0; i < channelCount; i++)
            {
                Channel.Add(i + 1, ChannelVolume);
            }
        }


        protected override void OnInit()
        {
            //if get Scene Exit Event then clear the Channel 
            Channel.Clear();
        }
    }
}