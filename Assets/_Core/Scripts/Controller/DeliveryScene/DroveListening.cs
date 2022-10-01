using System;
using System.Collections;
using System.Collections.Generic;
using Drove.Command.Command;
using QFramework;
using _Core.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

namespace Drove
{
    public class DroveListening : MonoBehaviour, IController
    {
        private List<PressedKeyCode> pressedKeyCode;
        [SerializeField] KeyCode SpeedUp = KeyCode.Space;
        [SerializeField] KeyCode SpeedDown = KeyCode.LeftShift;
        [SerializeField] KeyCode Forward = KeyCode.W;
        [SerializeField] KeyCode Back = KeyCode.S;
        [SerializeField] KeyCode Left = KeyCode.A;
        [SerializeField] KeyCode Right = KeyCode.D;
        [SerializeField] KeyCode TurnLeft = KeyCode.Q;
        [SerializeField] KeyCode TurnRight = KeyCode.E;
        private KeyCode[] keyCodes;
        public Action<PressedKeyCode[]> OnPressed;
        private void Awake()
        {
            keyCodes = new[]
            {
                SpeedUp,
                SpeedDown,
                Forward,
                Back,
                Left,
                Right,
                TurnLeft,
                TurnRight
            };
            pressedKeyCode = new List<PressedKeyCode>();
        }


        public void Init()
        {
            //this method is to provide to Upper Controller
            StopCoroutine(Listening());
            StartCoroutine(Listening());
        }

        public void Suspend()
        {
            pressedKeyCode.Clear();
            StopAllCoroutines();
        }

        IEnumerator Listening()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                ListeningKeyboard();
            }
        }

        void ListeningKeyboard()
        {
            for (int index = 0; index < keyCodes.Length; index++)
            {
                var keyCode = keyCodes[index];
                if (Input.GetKey(keyCode))
                    pressedKeyCode.Add((PressedKeyCode) index);
            }
            if (pressedKeyCode.Count != 0)
            {
                OnPressed.Invoke(pressedKeyCode.ToArray());
                pressedKeyCode.Clear();
            }
        }
        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}