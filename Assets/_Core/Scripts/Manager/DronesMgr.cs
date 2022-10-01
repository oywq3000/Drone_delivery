using System.Collections.Generic;

using _Core.Drove.Event;
using _Core.Drove.Script.Interface;

using QFramework;
using _Core.Scripts;
using UnityEngine;
namespace _Core.Drove.Script.Manager
{
    public class DronesMgr:MonoBehaviour,IController
    {
        private List<IAutoDrone> alreadyDrone;
        private List<IAutoDrone> occupyedDrone;

        private void Awake()
        {
            alreadyDrone = new List<IAutoDrone>();
            occupyedDrone = new List<IAutoDrone>();;
            
            //Register a Drone Initial Event
            this.RegisterEvent<ReigsterMySelf>(e =>
            {
                alreadyDrone.Add(e.DroneControllor);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
        }
        public IArchitecture GetArchitecture()
        {
           return DroneArchitecture.Interface;
        }
    }
}