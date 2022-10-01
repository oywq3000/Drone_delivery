
using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;

namespace Drove
{
    public interface ISceneCameraModel : IModel
    {
        List<ISceneCameraController> SceneCameraList { get; set; }
      
    }

    public class SceneCameraModel : AbstractModel,ISceneCameraModel
    {
        public List<ISceneCameraController> SceneCameraList { get; set; } = new List<ISceneCameraController>();
        protected override void OnInit()
        {
            DroneArchitecture.Interface.RegisterEvent<OnSceneExit>(e =>
            { 
                SceneCameraList.Clear();
            });
        }
        
    }
}