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
        private Coroutine NotTriggerHolder;

        private void Start()
        {
            onTrigger = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Drone"))
            {
                onTrigger = true;
                if (NotTriggerHolder!=null)
                {
                    StopCoroutine(NotTriggerHolder);
                }
                NotTriggerHolder = StartCoroutine(NotTrigger());
            }
           
        }

        IEnumerator NotTrigger()
        {
            yield return new WaitForEndOfFrame();
            onTrigger = false;
        }
    }
}