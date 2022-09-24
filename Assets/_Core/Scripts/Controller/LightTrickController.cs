using System.Collections;
using System.Collections.Generic;
using _Core.Drove.Event;
using Drove;
using QFramework;
using UnityEngine;
using UnityEngine.AI;

namespace _Core.Scripts.Controller
{
    public class LightTrickController : MonoBehaviour,IController
    {
        [SerializeField] public AutoController AutoController;
        private LineRenderer lineRenderer;
        private Vector3[] _vectorList;
        private Coroutine LightNavigationHolder;

        private void Start()
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
            _vectorList = new Vector3[2];

            this.RegisterEvent<EnableLigthNav>(e =>
            {
                if (e.IsEnable)
                {
                    lineRenderer.enabled = true;
                    LightNavigationHolder = StartCoroutine(LightNavigation());
                }
                else
                {
                    lineRenderer.enabled = false;
                   if(LightNavigationHolder!=null) StopCoroutine(LightNavigationHolder);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        
        IEnumerator LightNavigation()
        {
            while (true)
            {
                if (AutoController.flightStates == AutoController.FlightStates.Rest)
                {
                    yield break;
                }
                if (AutoController.attitudeStates == AutoController.States.Forward
                ||AutoController.attitudeStates == AutoController.States.Turn)
                {
                    _vectorList[0] = transform.position;
                }
                else
                {
                    _vectorList[0] = transform.parent.position;
                }
             

                if (AutoController.TargetPosition != null)
                {
                    var targetPosition = AutoController.TargetPosition;
                    Vector3 temp;
                    if (targetPosition.y != 0)
                    {
                        temp = new Vector3(_vectorList[0].x, targetPosition.y, _vectorList[0].z);
                    }
                    else
                    {
                        temp = new Vector3(targetPosition.x, _vectorList[0].y, targetPosition.z);
                    }

                    _vectorList[1] = temp;
                }
                else
                {
                    _vectorList[1] = _vectorList[0];
                }
                lineRenderer.SetPositions(_vectorList);
                yield return new WaitForFixedUpdate();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}