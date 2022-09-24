using System;
using System.Collections;
using System.Threading;
using _Core.Scripts.Utility;
using Drove;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;

namespace _Core.Logistics
{
    public class DesController : MonoBehaviour, IController
    {
        public string Id;
        private Vector3 _pos;
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _pos = transform.position;
            var desList = this.GetModel<ICargoDesMap>().DesList;
            desList.Add(this);
            Id = InitKit.GenerateId(desList.Count);
        } 
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Cargo"))
            {
                other.transform.SetParent(transform);
                other.GetOrAddComponent<Rigidbody>();
            }
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