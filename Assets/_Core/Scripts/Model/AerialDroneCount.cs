
using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using _Core.Scripts;
using UnityEngine;

namespace Drove
{
    public interface IAerialDroneCount : IModel
    {
        List<float> ChannelList { get; set; }
    }

    public class AerialDroneCount : AbstractModel,IAerialDroneCount
    {
        //initial variable
        public List<float> ChannelList { get; set; } = new List<float>();
        protected override void OnInit()
        {
            //if get Scene Exit Event then clear the ChannelList 
            DroneArchitecture.Interface.RegisterEvent<OnSceneExit>(e =>
            { 
                ChannelList.Clear();
            });
        }
    }
}