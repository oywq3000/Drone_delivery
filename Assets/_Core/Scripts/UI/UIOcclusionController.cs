using System;
using System.Collections.Generic;
using _Core.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using _Core.Drove.Script.System;

namespace QFramework.Example
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIOcclusionController : MonoBehaviour, IController
    {
        //下面是用来UI事件和射线
        private EventSystem eventSystem;
        private GraphicRaycaster RaycastInCanvas; //Canvas上有这个组件

        private bool isOcclusion;

        private bool IsOcclusion
        {
            get => isOcclusion;
            set
            {
                if (value != isOcclusion)
                {
                    isOcclusion = value;
                    this.GetSystem<IInputSystem>().InterruptDroneOperation(isOcclusion);
                }
            }
        }

        private void Start()
        {
            RaycastInCanvas = GetComponent<GraphicRaycaster>();
            eventSystem = GetComponentInChildren<EventSystem>();
        }

        private void Update()
        {
            //Check that UI is onTriggering 
            IsOcclusion = CheckGuiRaycastObjects();
        }

        public bool CheckGuiRaycastObjects() //测试UI射线
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list = new List<RaycastResult>();
            RaycastInCanvas.Raycast(eventData, list);
            //Debug.Log(list.Count);
            return list.Count > 0;
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}