using System;
using _Core.Drove.Event;
using _Core.Scripts.Utility;
using Cinemachine;
using Drove;
using FreeLookCustom;
using QFramework;
using _Core.Scripts.Controller;
using UnityEngine;


namespace _Core.Scripts.Controller
{
    public interface ISceneCameraController
    {
        string Id { get; }
        GameObject GameObject { get; }
    }

    public class SceneCameraController : FreeLookAbstrctController, IController, ISceneCameraController
    {
        public string Id
        {
            get => id;
        }

        [SerializeField] private string id;

        public GameObject GameObject
        {
            get => _gameObject;
        }

        private GameObject _gameObject;


        private float x_Value;
        private float y_Value;
        private float cameraScale;

        private bool isInterrupt = false;

        protected override void Start()
        {
            base.Start();
            x_Value = m_FreeLook.m_XAxis.Value;
            y_Value = m_FreeLook.m_YAxis.Value;
            cameraScale = m_CameraScale;
            this.RegisterEvent<DroneOperationListener>(e => { isInterrupt = e.IsInterrupt; });

            //Initiate Id and join the List of SceneCameraModel
            var sceneCameraControllers = this.GetModel<ISceneCameraModel>().SceneCameraList;
            //id = InitKit.GenerateId(sceneCameraControllers.Count);
            sceneCameraControllers.Add(this);

            _gameObject = gameObject;
        }

        protected override void Update()
        {
            if (isInterrupt)
            {
                //Interrupt the Listening
                return;
            }

            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition); //发出射线

                if (Physics.Raycast(ray, out hit))
                {
                    var m_TempGameObj = hit.collider.gameObject;
                    if (m_TempGameObj != transform.parent.gameObject ||
                        m_TempGameObj.transform.parent.parent.gameObject != transform.parent.gameObject)
                    {
                        if (m_TempGameObj.CompareTag("Drone"))
                        {
                            var pointCamera = m_TempGameObj.GetComponent<DroneController>().PointCamera;
                            //Trigger Event
                            DroneArchitecture.Interface.SendEvent(new BeforeCameraBlend()
                                {CurrentCamera = this.transform.position, TargentCamera = pointCamera.Position()});

                            pointCamera.SetActive(true);
                            this.gameObject.SetActive(false);
                        }

                        if (m_TempGameObj.CompareTag("Radar"))
                        {
                            if (m_TempGameObj.transform.parent.parent.gameObject != transform.parent.gameObject)
                            {
                                var pointCamera = m_TempGameObj.transform.parent.parent.GetComponent<DroneController>()
                                    .PointCamera;
                                DroneArchitecture.Interface.SendEvent(new BeforeCameraBlend()
                                    {CurrentCamera = this.transform.position, TargentCamera = pointCamera.Position()});
                                pointCamera
                                    .SetActive(true);
                                this.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (!m_FreeLook)
            {
                return;
            }

            m_FreeLook.m_XAxis.Value = x_Value;
            m_FreeLook.m_YAxis.Value = y_Value;
            m_FreeLook.m_Orbits[1].m_Radius = cameraScale * (m_MidRadius / 50f);
            m_FreeLook.m_Orbits[0].m_Radius = cameraScale * (m_TopRadius / 50f);
            m_FreeLook.m_Orbits[2].m_Radius = cameraScale * (m_BottomRadius / 50f);
            m_FreeLook.m_Orbits[0].m_Height = cameraScale * (m_TopHeight / 50f);
            m_FreeLook.m_Orbits[2].m_Height = cameraScale * (-m_BottomHeight / 50f);
        }

        public void SetScale(float degree)
        {
            m_CameraScale = degree;
            m_FreeLook.m_Orbits[1].m_Radius = m_CameraScale * (m_MidRadius / 50f);
            m_FreeLook.m_Orbits[0].m_Radius = m_CameraScale * (m_TopRadius / 50f);
            m_FreeLook.m_Orbits[2].m_Radius = m_CameraScale * (m_BottomRadius / 50f);
            m_FreeLook.m_Orbits[0].m_Height = m_CameraScale * (m_TopHeight / 50f);
            m_FreeLook.m_Orbits[2].m_Height = m_CameraScale * (-m_BottomHeight / 50f);
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}