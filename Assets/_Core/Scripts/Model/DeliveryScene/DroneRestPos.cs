
using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;

namespace Drove
{
    public interface IDroneRestPos : IModel
    {
        List<RestPlatform> RestList { get; set; }
    }

    public class DroneRestPos : AbstractModel, IDroneRestPos
    {

        public List<RestPlatform> RestList { get; set; } = new List<RestPlatform>();
        protected override void OnInit()
        {
            DroneArchitecture.Interface.RegisterEvent<OnSceneExit>(e =>
            { 
                RestList.Clear();
            });
        }

       
    }
}