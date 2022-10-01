using _Core.Drove.Event;
using Cinemachine;
using QFramework;
using _Core.Scripts;
using UnityEngine;

namespace Drove.Command.Command
{
    public class SwitchSceneCamera : AbstractCommand
    {
        private string _sceneId;

        public SwitchSceneCamera(string sceneId)
        {
            _sceneId =  sceneId;
            Debug.Log("SceneId:"+_sceneId);
        }
        protected override void OnExecute()
        {
            var _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            var sceneCameraList = this.GetModel<ISceneCameraModel>().SceneCameraList;
            var cinemachineBlendDefinition = _cinemachineBrain.m_DefaultBlend;
            foreach (var VARIABLE in sceneCameraList)
            {
                if (VARIABLE.Id == _sceneId)
                {
                    var virtualCameraGameObject = _cinemachineBrain.ActiveVirtualCamera
                        .VirtualCameraGameObject;

                    DroneArchitecture.Interface.SendEvent(new BeforeCameraBlend()
                    {
                        CurrentCamera = virtualCameraGameObject.transform.position,
                        TargentCamera = VARIABLE.GameObject.transform.position
                    });
                    virtualCameraGameObject.SetActive(false);
                    VARIABLE.GameObject.SetActive(true);
                    return;
                }
            }
        }
    }
}