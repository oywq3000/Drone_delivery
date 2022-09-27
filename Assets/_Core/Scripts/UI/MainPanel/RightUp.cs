/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/
using System;
using System.Collections;
using _Core.Drove.Event;
using _Core.Scripts;
using Cinemachine;
using Drove;
using UnityEngine;
using _Core.Scripts.Controller;
namespace QFramework.Example
{
    public partial class RightUp : UIElement, IController
    {
        private IEnumerator _moniterDroneInfHolder;
        private String _showText;
        private DroneController _droneController;
        private bool _isGetCargo;
        private int _cargoCount;
        private int _completeCount;
        private float _timer;
        public Action<int, string> OnAllCargoHasSent;

        private void Awake()
        {
            _moniterDroneInfHolder = MoniterDroneInf();
            //Register Camera Change Event
            Camera.main.GetComponent<CinemachineBrain>().m_CameraActivatedEvent
                .AddListener((a, b) =>
                {
                    if (a.VirtualCameraGameObject.CompareTag("DroneCamera"))
                    {
                        DroneInf.text = " ";
                        _droneController = a.VirtualCameraGameObject.transform.parent.GetComponent<DroneController>();
                    }
                    else
                    {
                        DroneInf.text = " ";
                        _droneController = null;
                    }
                });
            //Get cargo count
            //InitCargoCount();
            _cargoCount = 64; //this is a time-saving behave if time permits then try complete auto-generation 
            //initiate
            CompleteCargo.text = String.Format("送货进度：{0}/{1}", _completeCount, _cargoCount);
            this.RegisterEvent<OnOneCargoHasSent>(e =>
            {
                _completeCount++;
                if (_completeCount == _cargoCount)
                {
                    //entrust the parent panel
                    OnAllCargoHasSent.Invoke(_cargoCount, TimeParsy(_timer));
                }
            });


            LiseningSwitch(true);
        }

        #region DroneInf

        public void LiseningSwitch(bool On)
        {
            if (this.enabled == false)
            {
                return;
            }

            if (On)
            {
                DroneInf.text = " ";
                StartCoroutine(_moniterDroneInfHolder);
            }
            else
            {
                DroneInf.text = " ";

                StopCoroutine(_moniterDroneInfHolder);
            }
        }

        IEnumerator MoniterDroneInf()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                //Record the process time
                _timer += Time.deltaTime;

                CompleteCargo.text =
                    String.Format("送货进度：{0}/{1} 耗时: {2}", _completeCount, _cargoCount, TimeParsy(_timer));

                //Flesh the DroneInf
                if (_droneController)
                {
                    var mission = Mission(_droneController.autoController.flightStates);
                    if (mission != null)
                    {
                        _showText = String.Format("速度:{0:F}m/s 姿态:{1} "
                                                  + "\n任务：{2}"
                                                  + "\n暂时目标:"
                                                  + "\n({3:F},{4:F},{5:F})"
                            , _droneController.rigidbody.velocity.magnitude
                            , _droneController.autoController.attitudeStates
                            , mission
                            , _droneController.autoController.TargetPosition.x
                            , _droneController.autoController.TargetPosition.y
                            , _droneController.autoController.TargetPosition.z);
                    }
                    else
                    {
                        _showText = String.Format("速度:{0:F}m/s 姿态:{1} "
                                                  + "\n暂时目标:"
                                                  + "\n({2:F},{3:F},{4:F})"
                            , _droneController.rigidbody.velocity.magnitude
                            , _droneController.autoController.attitudeStates
                            , _droneController.autoController.TargetPosition.x
                            , _droneController.autoController.TargetPosition.y
                            , _droneController.autoController.TargetPosition.z);
                    }

                    DroneInf.text = _showText;
                }
            }
        }

        private string Mission(AutoController.FlightStates states)
        {
            switch (states)
            {
                case AutoController.FlightStates.ToCargo:
                    return "寻物";
                    break;
                case AutoController.FlightStates.ToCargoDes:
                    return "送货";
                    break;
                case AutoController.FlightStates.ToRest:
                    return "返回";
                    break;
                default:
                    return null;
            }
        }

        #endregion

        void InitCargoCount()
        {
            _cargoCount = this.GetModel<ICargoDesMap>().CargoList.Count;
        }

        private string TimeParsy(float timer)
        {
            string str;
            if (timer <= 60)
            {
                str = String.Format("{0:F}s", timer);
            }
            else
            {
                str = String.Format("{0}m{1:F}s", (int) timer / 60, timer % 60);
            }

            return str;
        }


        public void SetInfTextActive(bool enable)
        {
            DroneInf.enabled = enable;
            CompleteCargo.enabled = enable;
        }
        protected override void OnBeforeDestroy()
        {
            LiseningSwitch(false);
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}