using System;
using System.Collections;
using UnityEngine;
using QFramework;
using _Core.Drove.Event;
using _Core.Scripts.Utility;
using Drove;

namespace _Core.Scripts.Controller
{
    public interface IDroneController
    {
        string Id { get; }
        GameObject PointCamera { get; }
    }

    public class DroneController : MonoBehaviour, IController, IDroneController
    {
        public string Id
        {
            get => id;
        }
        [SerializeField] private string id;
        public GameObject PointCamera { get; set; }
        public DroveListening droveListening;
        public AutoController autoController;
        public HookController hookController;
       
        public Rigidbody rigidbody;
        public GameObject pointCamera;
        public AudioSource audioSource;

        private bool isAuto = true;
        /*Speed*/
        public int ForwardBackward = 50;
        /*Speed*/
        public int Tilt = 50;
        /*Speed*/
        public int FlyLeftRight = 50;
        /*Speed*/
        public int UpDown = 50;
        public float VerticalMaxSpeed = 10;
        public float HorizenMaxSpeed = 10;
        public GameObject[] rotors;
        private float _currentSpeed = 0;
        private float _volume = 0.5f;
        private Vector3 DroneRotation;
        private IEnumerator DroneEngineHolder;
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            //set coroutine for Engine
            DroneEngineHolder = DroneEngine();
            InitMode(isAuto);
            //get a serial Id by the DroneCountModel
            id = InitKit.GenerateId(this.GetModel<IDroneCountModel>().DroneList.Count);
            //add this to the DroneCountModel list
            this.GetModel<IDroneCountModel>().DroneList.Add(this);
            Debug.Log("DroneCount:"+this.GetModel<IDroneCountModel>().DroneList.Count);
            PointCamera = pointCamera;
            //Register Audio Event
            this.RegisterEvent<OnSoundVolumeChanged>(e => { _volume = e.Volume; })
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            autoController.OnArrivedRestPlatform.Register(() =>
            {
                EngineSwitch(false);
            });
        }
        
        public void InitMode(bool isAgncy)
        {
            if (isAgncy)
            {
                droveListening.Suspend();
                autoController.Init();
                autoController.SimulatePressed += OnKeyPassed;
                //binding the OnCargoSeized Event to autoController
                StartCoroutine(DroneEngineHolder);
            }
            else
            {
                autoController.Suspend();
                droveListening.Init();
                droveListening.OnPressed += OnKeyPassed;
                StartCoroutine(DroneEngineHolder);
            }
        }
        private void EngineSwitch(bool isOn)
        {
            if (DroneEngineHolder != null)
            {
                if (isOn)
                {
                    StopCoroutine(DroneEngineHolder);
                    StartCoroutine(DroneEngineHolder);
                }
                else
                {
                    StopCoroutine(DroneEngineHolder);
                }
            }
        }

        IEnumerator DroneEngine()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (autoController.attitudeStates == AutoController.States.Up)
                {
                    audioSource.volume = _volume;
                    RotationForRotor(1000);
                }
                else if (autoController.attitudeStates == AutoController.States.Down)
                {
                    audioSource.volume = _volume / 2;
                    RotationForRotor(200);
                }
                else if (autoController.attitudeStates == AutoController.States.Forward ||
                         autoController.attitudeStates == AutoController.States.Turn)
                {
                    audioSource.volume = _volume * 0.7f;
                    RotationForRotor(500);
                }
                else
                {
                    audioSource.volume = _volume * 0.2f;
                    RotationForRotor(100);
                }

                DroneRotation = rigidbody.transform.localEulerAngles;
                Stabilize();
            }
        }
        public void OnKeyPassed(PressedKeyCode[] pressedKeyCodes)
        {
            foreach (var pressedKeyCode in pressedKeyCodes)
            {
                switch (pressedKeyCode)
                {
                    case PressedKeyCode.SpeedUpPressed:
                        if (rigidbody.velocity.y <= VerticalMaxSpeed)
                        {
                            rigidbody.AddRelativeForce(0, UpDown, 0);
                        }

                        break;
                    case PressedKeyCode.SpeedDownPressed:
                        if (-rigidbody.velocity.y <= VerticalMaxSpeed)
                        {
                            rigidbody.AddRelativeForce(0, UpDown / -1, 0);
                        }

                        break;
                    case PressedKeyCode.ForwardPressed:
                        if (Mathf.Abs(rigidbody.velocity.magnitude) <= HorizenMaxSpeed)
                        {
                            rigidbody.AddRelativeForce(0,
                                Mathf.Tan((float) (DroneRotation.x * (Math.PI / 180))) * ForwardBackward,
                                ForwardBackward);
                        }

                        rigidbody.AddRelativeTorque(10, 0, 0);


                        break;
                    case PressedKeyCode.BackPressed:
                        if (Mathf.Abs(rigidbody.velocity.magnitude) <= HorizenMaxSpeed)
                        {
                            rigidbody.AddRelativeForce(0,
                                Mathf.Tan((float) (DroneRotation.x * (Math.PI / 180))) * (ForwardBackward / -1),
                                ForwardBackward / -1);
                        }

                        rigidbody.AddRelativeTorque(-10, 0, 0);
                        break;
                    case PressedKeyCode.LeftPressed:
                        rigidbody.AddRelativeTorque(0, Tilt / -1, 0);
                        break;
                    case PressedKeyCode.RightPressed:
                        rigidbody.AddRelativeTorque(0, Tilt, 0);
                        break;
                    case PressedKeyCode.TurnLeftPressed:
                        rigidbody.AddRelativeForce(FlyLeftRight / -1,
                            -Mathf.Tan((float) (DroneRotation.z * (Math.PI / 180))) * (FlyLeftRight / -1), 0);
                        rigidbody.AddRelativeTorque(0, 0, 10);
                        break;
                    case PressedKeyCode.TurnRightPressed:
                        rigidbody.AddRelativeForce(FlyLeftRight,
                            -Mathf.Tan((float) (DroneRotation.z * (Math.PI / 180))) * FlyLeftRight,
                            0);
                        rigidbody.AddRelativeTorque(0, 0, -10);
                        break;
                }
            }
        }
        void Stabilize()
        {
            if (DroneRotation.z > 10 && DroneRotation.z <= 180)
            {
                rigidbody.AddRelativeTorque(0, 0, -10);
            } //if tilt too big(stabilizes drone on z-axis)

            if (DroneRotation.z > 180 && DroneRotation.z <= 350)
            {
                rigidbody.AddRelativeTorque(0, 0, 10);
            } //if tilt too big(stabilizes drone on z-axis)

            if (DroneRotation.z > 1 && DroneRotation.z <= 10)
            {
                rigidbody.AddRelativeTorque(0, 0, -3);
            } //if tilt not very big(stabilizes drone on z-axis)

            if (DroneRotation.z > 350 && DroneRotation.z < 359)
            {
                rigidbody.AddRelativeTorque(0, 0, 3);
            } //if tilt not very big(stabilizes drone on z-axis)


            if (DroneRotation.x > 10 && DroneRotation.x <= 180)
            {
                rigidbody.AddRelativeTorque(-10, 0, 0);
            } //if tilt too big(stabilizes drone on x-axis)

            if (DroneRotation.x > 180 && DroneRotation.x <= 350)
            {
                rigidbody.AddRelativeTorque(10, 0, 0);
            } //if tilt too big(stabilizes drone on x-axis)

            if (DroneRotation.x > 1 && DroneRotation.x <= 10)
            {
                rigidbody.AddRelativeTorque(-3, 0, 0);
            } //if tilt not very big(stabilizes drone on x-axis)

            if (DroneRotation.x > 350 && DroneRotation.x < 359)
            {
                rigidbody.AddRelativeTorque(3, 0, 0);
            } //if tilt not very big(stabilizes drone on x-axis)

            //Gravity.y balance 
            if (autoController.flightStates != AutoController.FlightStates.Rest)
            {
                rigidbody.AddForce(0, -Physics.gravity.y,
                    0);
            }
            else
            {
                rigidbody.AddForce(0, -Physics.gravity.y - 1,
                    0);
            }
        }
        void RotationForRotor(float Targetspeed)
        {
            foreach (var VARIABLE in rotors)
            {
                VARIABLE.transform.Rotate(Vector3.forward * Targetspeed, Space.Self);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}