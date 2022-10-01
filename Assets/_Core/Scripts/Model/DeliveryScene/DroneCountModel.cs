using System.Collections.Generic;
using System.Linq;
using _Core.Drove.Event;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;


namespace Drove
{
    public interface IDroneCountModel : IModel
    {
        List<IDroneController> DroneList { get; set; }
    }

    public class DroneCountModel : AbstractModel,IDroneCountModel
    {
        public List<IDroneController> DroneList { get; set; } = new List<IDroneController>();
        protected override void OnInit()
        {
            DroneArchitecture.Interface.RegisterEvent<OnSceneExit>(e =>
            { 
                DroneList.Clear();
            });
            DroneArchitecture.Interface.RegisterEvent<OnDroneToRest>(e =>
            {
                DroneList.Remove(e.DroneController);
                this.SendEvent(new DroneListChanged()
                {
                    DroneController =  e.DroneController
                });
            });
        }
    }
}