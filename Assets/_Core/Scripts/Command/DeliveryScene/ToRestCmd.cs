using System.Linq;
using _Core.Drove.Event;
using _Core.Scripts.Controller;
using Cinemachine;
using QFramework;
using UnityEngine;

namespace Drove.Command.Command
{
    public class ToRestCmd : AbstractCommand
    {
        private DroneController _droneController;

        public ToRestCmd(DroneController droneController)
        {
            _droneController = droneController;
        }

        protected override void OnExecute()
        {
            this.SendEvent(new OnDroneToRest() {DroneController = _droneController});
            _droneController.pointCamera.SetActive(false);
            if (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject == _droneController.pointCamera)
            {
                this.GetModel<IDroneCountModel>().DroneList.First().PointCamera.SetActive(true);
            }
        }
    }
}