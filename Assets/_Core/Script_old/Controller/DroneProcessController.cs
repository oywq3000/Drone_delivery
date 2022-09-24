using System;
using Drove;
using QFramework;
using _Core.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Core.Scripts
{
    public class DroneProcessController:MonoBehaviour,IController
    {
        public Button startButton;
        private void Awake()
        {
            startButton.onClick.AddListener(() =>
            {
                StartProcess();
            });
        }
        void StartProcess()
        {
            this.SendCommand(new StartAProcess());
        }
        public IArchitecture GetArchitecture()
        {
           return Architecture.Interface;
        }
    }
}