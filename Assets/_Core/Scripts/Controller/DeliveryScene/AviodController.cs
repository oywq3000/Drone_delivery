using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Drove.Script.System;
using Drove;
using QFramework;
using _Core.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Core.Scripts.Controller
{
    public class AviodController : MonoBehaviour
    {
        public bool onTrigger;

        //record once trigger count
        public int TriggerCount;
        private Coroutine _resetTriggerCountHolder;
        private DroneController _lastEntryTriggerDrone;

        private void Start()
        {
            onTrigger = false;
            TriggerCount = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Drone"))
            {
                //set trigger flag
                onTrigger = true;
                //store the last entry trigger drone
                _lastEntryTriggerDrone = other.GetComponent<DroneController>();
                if (_lastEntryTriggerDrone.autoController.attitudeStates ==AutoController.States.Forward||
                    _lastEntryTriggerDrone.autoController.attitudeStates == AutoController.States.HorizonInterruption)
                {
                    //only the two drone is in Forward or HorizonInterruption States the TriggerCount add
                    TriggerCount++;
                }
              
                //Start INT coroutine
                if (_resetTriggerCountHolder != null)
                {
                    StopCoroutine(_resetTriggerCountHolder);
                }
                _resetTriggerCountHolder= StartCoroutine(ResetTriggerCount());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Drone"))
            {
                onTrigger = false;
            }
        }

        public DroneController GetLastDrone()
        {
            return _lastEntryTriggerDrone;
        }

        IEnumerator ResetTriggerCount()
        {
            yield return new WaitForSeconds(3);
            //if not INT,then reset TriggerCount
            TriggerCount = 0;
        }
    }
}