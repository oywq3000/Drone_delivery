
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using Cinemachine;
using QFramework;
using _Core.Scripts.Controller;
using UnityEngine;

namespace Drove.Command.Command
{
    public class SetUltraViewCmd: AbstractCommand
    {
        private static bool isUltraView = true;
        private float _defaultView = 250;
        private float _ultraView = 2000;

       
        protected override void OnExecute()
        {
            //trigger the Event that 
          
            if (isUltraView)
            {
                this.SendCommand(new SetCameraScale(_ultraView));
            }
            else
            {
                var droneCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject
                    .GetComponent<DroneCamera>();
                if (droneCamera.m_CameraScale>=_ultraView)
                {
                    this.SendCommand(new SetCameraScale(_ultraView));
                }
                else
                {
                    this.SendCommand(new SetCameraScale(_ultraView));
                    isUltraView = !isUltraView;
                }
            }
            isUltraView = !isUltraView;
        }
    }
}