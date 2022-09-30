using System.Collections;
using System.Threading;
using Drove;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;

namespace _Core.Scripts.Controller
{
    public class RestPlatform : MonoBehaviour, IController
    {
        public string Id { get; set; }
        private Vector3 _pos;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _pos = transform.position;
            var restList = this.GetModel<IDroneRestPos>().RestList;
            restList.Add(this);
            Debug.Log("RestCount:" + restList.Count);
        }

        public Vector3 GetPos()
        {
            return new Vector3(_pos.x, _pos.y + 1, _pos.z);
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}