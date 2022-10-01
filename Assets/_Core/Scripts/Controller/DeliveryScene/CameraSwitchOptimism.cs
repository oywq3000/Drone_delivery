using System;
using _Core.Drove.Event;
using Cinemachine;
using QFramework;
using _Core.Scripts;
using UnityEngine;

namespace _Core.Scripts.Controller
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CameraSwitchOptimism:MonoBehaviour,IController
    {
        private CinemachineBrain _cinemachineBrain;

        private void Start()
        {
            _cinemachineBrain = GetComponent<CinemachineBrain>();
            //call before virtual camera switch
            this.RegisterEvent<BeforeCameraBlend>(e =>
            {
                //adjust BlendTime dynamically
                _cinemachineBrain.m_DefaultBlend.m_Time =Mathf.Log10((e.CurrentCamera - e.TargentCamera).magnitude+1);
            });
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}