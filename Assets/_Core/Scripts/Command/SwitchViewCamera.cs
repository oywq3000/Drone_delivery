using _Core.Drove.Event;
using Cinemachine;
using QFramework;
using UnityEngine;

namespace Drove.Command.Command
{
    public class SwitchViewCamera : AbstractCommand
    {
        private string _droneId;

        public SwitchViewCamera(string droneId, bool isFirst = false)
        {
            _droneId = droneId.PadLeft(4,'0');
            _isFirst = isFirst;
        }

        private bool _isFirst;

        protected override void OnExecute()
        {
            var _droneControllers = this.GetModel<IDroneCountModel>().DroneList;
            var _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            foreach (var VARIABLE in _droneControllers)
            {
                if (VARIABLE.Id == _droneId)
                {
                    if (_isFirst)
                    {
                        VARIABLE.PointCamera.SetActive(true);
                        return;
                    }
                    this.SendEvent(new BeforeCameraBlend()
                    {
                        CurrentCamera = _cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.position,
                        TargentCamera = VARIABLE.PointCamera.transform.position
                    });
                    _cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
                    VARIABLE.PointCamera.SetActive(true);
                    return;
                }
            }
        }
    }
}