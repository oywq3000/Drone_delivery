using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using Drove.Command.Command;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;

using UnityEngine;

using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Drove
{
    public struct ExecutePoints
    {
        public Vector3[] Points;
        public short Index;
    }

    public interface IAutoController
    {
        Vector3 TargetPosition { get; set; }
    }
    public class AutoController : MonoBehaviour, IController,IAutoController
    {
        public HookController hookController;
        public List<PressedKeyCode> demandPressedKeys;
        public States attitudeStates;
        public FlightStates flightStates;
        public Vector3 destination;
        public float channel;

        public AviodController UpSensor;
        public AviodController DownSensor;
        public AviodController HorizonSensor;

        private Transform _currentTsm;
        private Rigidbody _droneRigidbody;
        public Vector3 TargetPosition { get; set; }

        private ExecutePoints _executePoints;

        //this two holders is to record two coroutine for ending their corresponding coroutine
        private Coroutine _heightHolder;
        private Coroutine _forwardHolder;

        #region Event
        public EasyEvent OnArrivedRestPlatform = new EasyEvent();
        public Action<PressedKeyCode[]> SimulatePressed;
        #endregion
        public enum States
        {
            Idle,
            Up,
            Down,
            Turn,
            Forward,
            UpInterruption,
            DownInterruption,
            HorizonInterruption
        }
        public enum FlightStates
        {
            ToCargo,
            ToCargoDes,
            ToRest,
            Rest
        }

        public FSM<States> FSM = new FSM<States>();

        #region TestMethod

        #endregion

        public void Init()
        {
            _droneRigidbody = GetComponentInParent<Rigidbody>();
            //create StateMachine
            StateMachine();
            _currentTsm = transform.parent;
            StopCoroutine(Angencying());
            StartCoroutine(Angencying());
            //indicates the drones has already to be schedule 
            this.RegisterEvent<DroneStartDelivery>(SeekCargo)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            //add itself to Model
        }

        public void Suspend()
        {
            StopAllCoroutines();
            FSM.Clear();
        }

        private void SeekCargo(DroneStartDelivery e)
        {
            var cargoPosList = this.GetSystem<INavigationSystem>().GetCargoPos();
            channel = SetTargetPoint(cargoPosList);
            //change flight states
            flightStates = FlightStates.ToCargo;
        }

        IEnumerator OnArriveTarget()
        {
            var navigationSystem = this.GetSystem<INavigationSystem>();
            //auto set Target 
            yield return new WaitForSeconds(1f);
            switch (flightStates)
            {
                case FlightStates.ToCargo:
                    //Clear the flight channel
                    //IsEnable the collider for checking whether there is cargo
                    hookController.SetCollider(true);
                    //wait for that hook is get the cargo
                    yield return new WaitUntil(hookController.IsGetCargo);
                    var targetPosition = navigationSystem.GetCargoDes(hookController.GetCargoCtrl());
                    channel = SetTargetPoint(targetPosition);
                    flightStates = FlightStates.ToCargoDes;
                    hookController.SetCollider(false);
                    break;
                case FlightStates.ToCargoDes:
                    //drop the cargo
                    yield return new WaitForSeconds(2f);
                    this.SendCommand<CompleteOneCargoTransCmd>();
                    //set rest destination
                    var des = navigationSystem.GetCargoPos();
                    if (des == null)
                    {
                        //if cargo had delivered then go to rest
                        des = navigationSystem.GetRestPos();
                        channel = SetTargetPoint(des);
                        flightStates = FlightStates.ToRest;
                        break;
                    }
                    channel = SetTargetPoint(des);
                    flightStates = FlightStates.ToCargo;
                    break;
                case FlightStates.ToRest:
                    yield return new WaitForSeconds(1f);
                    //Trigger Event
                    OnArrivedRestPlatform.Trigger();
                    flightStates = FlightStates.Rest;
                    //stop this drone
                    StopAllCoroutines();
                    break;
            }
            yield return null;
        }

        private float SetTargetPoint(TargetPosition _target)
        {
            //assigning and parse
            var flyPoints = new List<Vector3>();
            flyPoints.Add(new Vector3(0, _target.FlightHeight, 0));
            flyPoints.Add(new Vector3(_target.x, 0, _target.z));
            flyPoints.Add(new Vector3(0, _target.y, 0));
            _executePoints.Points = flyPoints.ToArray();
            _executePoints.Index = 0;
            //indicates the destination in serialization
            destination = new Vector3(_target.x, _target.y, _target.z);
            //function and set a interruption
            FSM.StartState(States.Idle);
            return _target.FlightHeight;
        }

        private void StateMachine()
        {
            Quaternion targetRotation = Quaternion.identity;
            FSM.State(States.Idle).OnEnter(() =>
            {
                if (_executePoints.Index >= _executePoints.Points.Length)
                {
                    StopCoroutine(OnArriveTarget());
                    StartCoroutine(OnArriveTarget());
                    return;
                }

                //Action sequence
                TargetPosition = _executePoints.Points[_executePoints.Index++];

                if (TargetPosition.y != 0)
                {
                    //indicates TargetPosition is vertical direction
                    FSM.ChangeState(States.Up);
                }
                else
                {
                    //indicates TargetPosition is horizon direction

                    FSM.ChangeState(States.Turn);
                }
            });
            FSM.State(States.Up)
                .OnEnter(() =>
                {
                    if (_heightHolder != null) StopCoroutine(_heightHolder);
                })
                .OnFixedUpdate(() =>
                {
                    
                    if (TargetPosition.y - _currentTsm.position.y < 0)
                    {
                        FSM.ChangeState(States.Down);
                        return;
                    }

                    if (TargetPosition.y - _currentTsm.position.y == 0)
                    {
                        //send a command that drone alright complete its one instruction
                        FSM.ChangeState(States.Idle);
                        return;
                    }
                    var v = _droneRigidbody.velocity.y;
                    var a = (_droneRigidbody.drag - Physics.gravity.y - 7) /
                            _droneRigidbody.mass;
                    if (UpSensor.onTrigger)
                    {
                        FSM.ChangeState(States.UpInterruption);
                    }
                    else if (TargetPosition.y - _currentTsm.position.y > v * v / (2 * a))
                    {
                        DroveMove("10000000".ToCharArray());
                    }
                    else
                    {
                        _heightHolder = StartCoroutine(HeightCorrection(TargetPosition.y));
                        FSM.ChangeState(States.Idle);
                        return;
                    }
                });

            FSM.State(States.UpInterruption)
                .OnFixedUpdate(() =>
                {
                    if (UpSensor.onTrigger)
                    {
                        DroveMove("01000000".ToCharArray());
                    }
                    else
                    {
                        //if not signal return down state
                        FSM.ChangeState(States.Up);
                    }
                });

            FSM.State(States.Down)
                .OnEnter(() =>
                {
                    if (_heightHolder != null) StopCoroutine(_heightHolder);
                    if (channel > 0)
                        this.GetSystem<INavigationSystem>().ReleaseChannel((int)channel);
                })
                .OnFixedUpdate(() =>
                {
                    var v = _droneRigidbody.velocity.y;
                    var a = _droneRigidbody.drag - Physics.gravity.y - 7 /
                        _droneRigidbody.mass;

                    if (DownSensor.onTrigger)
                    {
                        FSM.ChangeState(States.DownInterruption);
                    }
                    else if (_currentTsm.position.y - TargetPosition.y > v * v / (2 * a))
                    {
                        DroveMove("01000000".ToCharArray());
                    }
                    else
                    {
                        _heightHolder = StartCoroutine(HeightCorrection(TargetPosition.y));
                        FSM.ChangeState(States.Idle);
                    }
                });

            FSM.State(States.DownInterruption)
                .OnFixedUpdate(() =>
                {
                    if (DownSensor.onTrigger)
                    {
                        DroveMove("10000000".ToCharArray());
                    }
                    else
                    {
                        //if not signal return down state
                        FSM.ChangeState(States.Down);
                    }
                });


            FSM.State(States.Turn)
                .OnEnter(() =>
                {
                    targetRotation = Quaternion.LookRotation(
                        new Vector3(TargetPosition.x - _currentTsm.position.x, 0,
                            TargetPosition.z - _currentTsm.position.z),
                        Vector3.up);
                })
                .OnFixedUpdate(() =>
                {
                    if (Quaternion.Angle(_currentTsm.rotation, targetRotation) < 1)
                    {
                        transform.parent.rotation = targetRotation;
                        FSM.ChangeState(States.Forward);
                        return;
                    }

                    transform.parent.rotation =
                        Quaternion.Slerp(_currentTsm.rotation, targetRotation, Time.deltaTime / 1.5f);
                });
            FSM.State(States.Forward)
                .OnEnter(() =>
                {
                    if (_forwardHolder != null) StopCoroutine(_forwardHolder);
                })
                .OnFixedUpdate(() =>
                {
                    Vector3 currentPos = _currentTsm.position;
                    if (currentPos.x - TargetPosition.x == 0 && currentPos.z - TargetPosition.z == 0)
                    {
                        //interruption
                        FSM.ChangeState(States.Idle);
                        return;
                    }

                    float des = Mathf.Abs(TargetPosition.x - currentPos.x);
                    var v = _droneRigidbody.velocity.x;
                    var a = (_droneRigidbody.drag + _droneRigidbody.angularDrag) / _droneRigidbody.mass;
                    if (v * v / (2 * a) >= des||des<0.2f)
                    {
                        _forwardHolder = StartCoroutine(ForwardCorrection(TargetPosition));
                        FSM.ChangeState(States.Idle);
                        return;
                    }

                    if (HorizonSensor.onTrigger)
                    {
                        FSM.ChangeState(States.HorizonInterruption);
                        return;
                    }

                    DroveMove("00100000".ToCharArray());
                });
            FSM.State(States.HorizonInterruption).OnFixedUpdate(() =>
            {
                if (HorizonSensor.onTrigger)
                {
                    DroveMove("00010000".ToCharArray());
                }
                else
                {
                    //if not signal return down state
                    FSM.ChangeState(States.Turn);
                }
            });
        }

        IEnumerator Angencying()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                //flesh every fixedFrame
                FSM.FixedUpdate();
                _currentTsm = transform.parent.transform;
                attitudeStates = FSM.CurrentStateId;
            }
        }

        #region DroveFlyFunction

        void DroveMove(char[] bools)
        {
            if (bools.Length < 8)
            {
                return;
            }

            if (bools[0] == '1')
            {
                //DroveUp
                demandPressedKeys.Add(PressedKeyCode.SpeedUpPressed);
            }

            if (bools[1] == '1')
            {
                //DroveDown
                demandPressedKeys.Add(PressedKeyCode.SpeedDownPressed);
            }

            if (bools[2] == '1')
            {
                //DroveForward
                demandPressedKeys.Add(PressedKeyCode.ForwardPressed);
            }

            if (bools[3] == '1')
            {
                //DroveBack
                demandPressedKeys.Add(PressedKeyCode.BackPressed);
            }

            if (bools[4] == '1')
            {
                //DroveLeft
                demandPressedKeys.Add(PressedKeyCode.LeftPressed);
            }

            if (bools[5] == '1')
            {
                //DroveRight
                demandPressedKeys.Add(PressedKeyCode.RightPressed);
            }

            if (bools[6] == '1')
            {
                //DroveLeftward
                demandPressedKeys.Add(PressedKeyCode.TurnLeftPressed);
            }

            if (bools[7] == '1')
            {
                //DroveRightward()
                demandPressedKeys.Add(PressedKeyCode.TurnRightPressed);
            }

            //call the drone movement
            if (demandPressedKeys.Count != 0)
            {
                //fresh the DroveMovement every frame that is given
                // droneControllor.OnKeyPassed(demandPressedKeys.ToArray());
                SimulatePressed.Invoke(demandPressedKeys.ToArray());
                demandPressedKeys.Clear();
            }
        }

        IEnumerator HeightCorrection(float targetHeigh)
        {
            Vector3 pos;
            while (true)
            {
                yield return null;
                if (Mathf.Abs(_droneRigidbody.velocity.y) > 0.1f)
                {
                    continue;
                }
                pos = _currentTsm.position;
                pos = new Vector3(pos.x
                    , Mathf.Lerp(pos.y,
                        targetHeigh,
                        Time.deltaTime), pos.z);
                transform.parent.position = pos;
                if (Mathf.Abs(pos.y - targetHeigh) < 0.05f)
                {
                    transform.position = new Vector3(pos.x,
                        targetHeigh, pos.z);
                    yield break;
                }
            }
        }

        IEnumerator ForwardCorrection(Vector3 target)
        {
            Vector3 pos;
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (Mathf.Abs(_droneRigidbody.velocity.x) >0.1f || Mathf.Abs(_droneRigidbody.velocity.z) > 0.1f)
                {
                    continue;
                }
                pos = _currentTsm.position;

                pos = new Vector3(Mathf.Lerp(pos.x,
                        target.x,
                        Time.deltaTime)
                    , pos.y,
                    Mathf.Lerp(pos.z,
                        target.z,
                        Time.deltaTime));
                transform.parent.position = pos;
                if (Mathf.Abs(pos.z - target.z) < 0.01f||Mathf.Abs(pos.x - target.x)<0.01f)
                {
                    transform.position = new Vector3(target.x,
                        pos.y, target.z);
                }
            }
        }

        #endregion

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}