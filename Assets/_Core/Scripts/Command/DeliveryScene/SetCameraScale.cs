using Cinemachine;
using QFramework;
using _Core.Scripts.Controller;
using UnityEngine;

namespace Drove.Command.Command
{
    public class SetCameraScale : AbstractCommand
    {
        private float _degree;
        public SetCameraScale(float degree)
        {
            _degree = degree;
        }
        protected override void OnExecute()
        {
            var _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            var virtualCameraGameObject =
                _cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;
            if (virtualCameraGameObject.CompareTag("DroneCamera"))
            {
                virtualCameraGameObject.GetComponent<DroneCamera>().SetScale(_degree);
            }
            else if (virtualCameraGameObject.CompareTag("SceneCamera"))
            {
                virtualCameraGameObject.GetComponent<SceneCameraController>().SetScale(_degree);
            }
        }
    }
}