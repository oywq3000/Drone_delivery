using System;
using Drove;
using QFramework;
using UnityEngine;
namespace _Core.Scripts.Controller
{
    public class HookController:MonoBehaviour
    {

        public Action<TargetPosition> OnCargoSeized;
        private CargoController _seizedCargo;
        private bool getCargo = false;
        private Collider _collider;

        private void Start()
        {
            //default off
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Cargo"))
            {
                other.transform.SetParent(this.transform);
                //trigger the signal;
                _seizedCargo = other.GetComponent<CargoController>();
                getCargo = true;
            }
        }

        public void SetCollider(bool isEnable)
        {
            _collider.enabled = isEnable;
            getCargo = false;
        }
        
        
        public bool IsGetCargo()
        {
            return getCargo;
        }
        public CargoController GetCargoCtrl()
        {
            return _seizedCargo;
        }
        public void DischangeCargo()
        {
            //Dischange Cargo;
            _seizedCargo?.transform.SetParent(null);
        }
     
    }
}