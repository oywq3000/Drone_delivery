using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using UnityEngine;

namespace _Core.Drove.Script.System
{
    /// <summary>
    /// client customize the input buttion
    /// </summary>
    /// <summary>
    /// Interrupt layer, client customize
    /// </summary>
    public enum InputLayer
    {
        EngagingGame,
        UI
    }

    public interface IInputSystem : ISystem
    {
        //Register Key Event
        List<Dictionary<KeyCode, List<Action>>> KeyCodeActiveLayer { get; }
        List<Dictionary<KeyCode, List<Action>>> KeyCodeDownActiveLayer { get; }

        void RegisterGetKey(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void UnRegisterGetKey(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void RegisterGetKeyDown(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void UnRegisterGetKeyDown(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        //Register and UnRegister the Axis Event
        void RegisterAxis(string axisName, Action<float> action, InputLayer targetLayer = InputLayer.EngagingGame);
        void UnRegisterAxis(string axisName, Action<float> action, InputLayer targetLayer = InputLayer.EngagingGame);

        void UpdateHolder();
        void SetGetKeyINT(InputLayer targetLayer);
        void RecoveryGetKeyINT(InputLayer targetLayer);
        void SetGetKeyDownINT(InputLayer targetLayer);
        void RecoveryGetKeyDownINT(InputLayer targetLayer);
        void SetGetAxisINT(InputLayer targetLayer);
        void RecoverySetGetAxisINT(InputLayer targetLayer);
        void InterruptDroneOperation(bool isInterrupt);
    }

    public class InputSystem : AbstractSystem, IInputSystem
    {
        public List<UnityEngine.KeyCode> KeyCodes = new List<UnityEngine.KeyCode>();

        public List<Dictionary<KeyCode, List<Action>>> KeyCodeActiveLayer { get; } =
            new List<Dictionary<KeyCode, List<Action>>>();

        public List<Dictionary<KeyCode, List<Action>>> KeyCodeDownActiveLayer { get; } =
            new List<Dictionary<KeyCode, List<Action>>>();

        public List<Dictionary<string, List<Action<float>>>> AxisActiveLayer { get; } =
            new List<Dictionary<string, List<Action<float>>>>();

        //restore the InoutLayer & Dictionary to a Dictionary
        private Dictionary<InputLayer, Dictionary<KeyCode, List<Action>>> RegisteredKeyCodeDic =
            new Dictionary<InputLayer, Dictionary<KeyCode, List<Action>>>();

        private Dictionary<InputLayer, Dictionary<KeyCode, List<Action>>> RegisteredKeyCodeDownDic =
            new Dictionary<InputLayer, Dictionary<KeyCode, List<Action>>>();

        private Dictionary<InputLayer, Dictionary<string, List<Action<float>>>> RegisteredAxisDic =
            new Dictionary<InputLayer, Dictionary<string, List<Action<float>>>>();

        //this list will restore your registered GetKey and GetKeyDown
        private List<KeyCode> registeredKeyCodes = new List<KeyCode>();

        private List<KeyCode> registeredKeyCodeDowns = new List<KeyCode>();

        //this list will restore axis that you register
        private List<string> registeredAxis = new List<string>();

        protected override void OnInit()
        {
        }

        //this is keycode listener, client need put it to a Update function of Monobehavior 
        public void UpdateHolder()
        {
            if (Input.anyKey)
            {
                //KeyCode Event
                //travel all active layer and find pressed key action then invoke it
                foreach (var dic in KeyCodeActiveLayer)
                {
                    foreach (var dicKey in dic.Keys)
                    {
                        if (Input.GetKey(dicKey))
                        {
                            if (dic.TryGetValue(dicKey, out List<Action> actions))
                            {
                                foreach (var action in actions)
                                {
                                    action?.Invoke();
                                }
                            }
                        }
                    }
                }
            }

            if (Input.anyKeyDown)
            {
                //travel all active layer and find pressed key action then invoke it
                foreach (var dic in KeyCodeDownActiveLayer)
                {
                    foreach (var dicKey in dic.Keys)
                    {
                        if (Input.GetKeyDown(dicKey))
                        {
                            if (dic.TryGetValue(dicKey, out List<Action> actions))
                            {
                                foreach (var action in actions)
                                {
                                    action?.Invoke();
                                }
                            }
                        }
                    }
                }
            }
            //Axis Event
            foreach (var dic in AxisActiveLayer)
            {
                foreach (var dicKey in dic.Keys)
                {
                    if (Input.GetAxis(dicKey) != 0)
                    {
                        if (dic.TryGetValue(dicKey, out List<Action<float>> actions))
                        {
                            foreach (var action in actions)
                            {
                                action?.Invoke(Input.GetAxis(dicKey));
                            }
                        }
                    }
                }
            }
        }

        public void RegisterGetKey(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Add(action);
                }
                else
                {
                    var list = new List<Action>();
                    list.Add(action);
                    valueDic.Add(keyCode, list);
                }
            }

            else
            {
                var dictionary = new Dictionary<KeyCode, List<Action>>();
                var list = new List<Action>();
                list.Add(action);
                dictionary.Add(keyCode, list);
                //add to registeredkeycode
                RegisteredKeyCodeDic.Add(targetLayer, dictionary);
                //when input layer is first created, add it to active list, which is defualt
                KeyCodeActiveLayer.Add(dictionary);
            }
        }

        public void UnRegisterGetKey(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Remove(action);
                    if (actions.Count == 0)
                    {
                        valueDic.Remove(keyCode);
                    }
                }
            }
        }

        public void RegisterGetKeyDown(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Add(action);
                }
                else
                {
                    var list = new List<Action>();
                    list.Add(action);
                    valueDic.Add(keyCode, list);
                }
            }
            else
            {
                var dictionary = new Dictionary<KeyCode, List<Action>>();
                var list = new List<Action>();
                list.Add(action);
                dictionary.Add(keyCode, list);
                RegisteredKeyCodeDownDic.Add(targetLayer, dictionary);
                //when input layer is first created, add it to active list, which is defualt
                KeyCodeDownActiveLayer.Add(dictionary);
            }
        }

        public void UnRegisterGetKeyDown(KeyCode keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Remove(action);
                    if (actions.Count == 0)
                    {
                        valueDic.Remove(keyCode);
                    }
                }
            }
        }

        public void RegisterAxis(string axisName, Action<float> action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            if (RegisteredAxisDic.TryGetValue(targetLayer, out Dictionary<string, List<Action<float>>> valueDic))
            {
                if (valueDic.TryGetValue(axisName, out List<Action<float>> actions))
                {
                    actions.Add(action);
                }
                else
                {
                    var list = new List<Action<float>>();
                    list.Add(action);
                    valueDic.Add(axisName, list);
                }
            }
            else
            {
                var dictionary = new Dictionary<string, List<Action<float>>>();
                var list = new List<Action<float>>();
                list.Add(action);
                dictionary.Add(axisName, list);
                RegisteredAxisDic.Add(targetLayer, dictionary);
                //when input layer is first created, add it to active list, which is defualt
                AxisActiveLayer.Add(dictionary);
            }
        }

        public void UnRegisterAxis(string axisName, Action<float> action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            if (RegisteredAxisDic.TryGetValue(targetLayer, out Dictionary<string, List<Action<float>>> valueDic))
            {
                if (valueDic.TryGetValue(axisName, out List<Action<float>> actions))
                {
                    actions.Remove(action);
                    if (actions.Count == 0)
                    {
                        valueDic.Remove(axisName);
                    }
                }
            }
        }

//Interruption Method
        public void InterruptDroneOperation(bool isInterrupt)
        {
            this.SendEvent(new DroneOperationListener() {IsInterrupt = isInterrupt});
        }

        public void SetGetKeyINT(InputLayer targetLayer)
        {
            //remove this layer dic 
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                KeyCodeActiveLayer?.Remove(valueDic);
            }
        }

        public void RecoveryGetKeyINT(InputLayer targetLayer)
        {
            //Recovery this layer
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (!KeyCodeActiveLayer.Contains(valueDic))
                {
                    KeyCodeActiveLayer.Add(valueDic);
                }
            }
        }

        public void SetGetKeyDownINT(InputLayer targetLayer)
        {
            //remove this layer dic 
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                KeyCodeDownActiveLayer?.Remove(valueDic);
                Debug.Log("ActiveCount: " + KeyCodeActiveLayer.Count);
            }
        }

        public void RecoveryGetKeyDownINT(InputLayer targetLayer)
        {
            //Recovery this layer
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<KeyCode, List<Action>> valueDic))
            {
                if (!KeyCodeActiveLayer.Contains(valueDic))
                {
                    KeyCodeDownActiveLayer.Add(valueDic);
                }
            }
        }

        public void SetGetAxisINT(InputLayer targetLayer)
        {
            //remove this layer dic 
            if (RegisteredAxisDic.TryGetValue(targetLayer, out Dictionary<string, List<Action<float>>> valueDic))
            {
                AxisActiveLayer?.Remove(valueDic);
            }
        }

        public void RecoverySetGetAxisINT(InputLayer targetLayer)
        {
            //Recovery this layer
            if (RegisteredAxisDic.TryGetValue(targetLayer, out Dictionary<string, List<Action<float>>> valueDic))
            {
                if (!AxisActiveLayer.Contains(valueDic))
                {
                    AxisActiveLayer.Add(valueDic);
                }
            }
        }
    }
}