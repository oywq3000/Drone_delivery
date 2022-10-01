using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Drove.Script.System;
using _Core.Scripts.Utility;
using Drove;
using QFramework;
using _Core.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Core.Scripts.Controller
{
    public class CargoController : MonoBehaviour, IController
    {
        public string Id;
        private Vector3 _pos;
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            //wait for rendering 
            _pos = transform.position;
            var cargoList = this.GetModel<ICargoDesMap>().CargoList;
            cargoList.Add(this);
            //Generate a Id by serialization
            Id =  InitKit.GenerateId(cargoList.Count);
        }
        public Vector3 GetPos()
        {
            return _pos;
        }
        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}