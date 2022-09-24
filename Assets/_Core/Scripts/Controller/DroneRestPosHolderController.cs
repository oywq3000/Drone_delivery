using System;
using QFramework;
using _Core.Scripts;
using UnityEngine;

namespace _Core.Logistics
{
    public class DroneRestPosHolderController:MonoBehaviour,IController
    {
        private Vector3 _posHolder;
        public string Id { get; set; }
        private void Awake()
        {
            _posHolder = this.gameObject.transform.position;
        }

        private void Start()
        {
            
        }
        
        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}