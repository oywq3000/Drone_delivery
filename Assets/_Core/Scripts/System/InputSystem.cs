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
    public enum ClientKeys
    {
        Mouse0 = KeyCode.Mouse0,
        Mouse1 = KeyCode.Mouse1,
    }

    /// <summary>
    /// Interrupt layer, client customize
    /// </summary>
    public enum InputLayer
    {
        EngagingGame
    }

    public interface IInputSystem : ISystem
    {
        //Register Key Event
        List<Dictionary<ClientKeys, List<Action>>> KeyCodeActiveLayer { get; }
        List<Dictionary<ClientKeys, List<Action>>> KeyCodeDownActiveLayer { get; }

        void RegisterGetKey(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void UnRegisterGetKey(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void RegisterGetKeyDown(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void UnRegisterGetKeyDown(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame);

        void UpdateHolder();

        void InterruptDroneOperation(bool isInterrupt);
    }

    public class InputSystem : AbstractSystem, IInputSystem
    {
        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public List<Dictionary<ClientKeys, List<Action>>> KeyCodeActiveLayer { get; } =
            new List<Dictionary<ClientKeys, List<Action>>>();

        public List<Dictionary<ClientKeys, List<Action>>> KeyCodeDownActiveLayer { get; } =
            new List<Dictionary<ClientKeys, List<Action>>>();

        //restore the InoutLayer & Dictionary to a Dictionary
        private Dictionary<InputLayer, Dictionary<ClientKeys, List<Action>>> RegisteredKeyCodeDic =
            new Dictionary<InputLayer, Dictionary<ClientKeys, List<Action>>>();

        private Dictionary<InputLayer, Dictionary<ClientKeys, List<Action>>> RegisteredKeyCodeDownDic =
            new Dictionary<InputLayer, Dictionary<ClientKeys, List<Action>>>();

        protected override void OnInit()
        {
        }


        //this is keycode listener, client need put it to a Update function of Monobehavior 
        public void UpdateHolder()
        {
            if (Input.anyKey)
            {
                foreach (ClientKeys clientKey in Enum.GetValues(typeof(ClientKeys)))
                {
                    //if get the clientKey 
                    if (Input.GetKey((KeyCode) clientKey))
                    {
                        //travel all active layer to find pressed key action then invoke it
                        foreach (var dic in KeyCodeActiveLayer)
                        {
                            if (dic.TryGetValue(clientKey, out List<Action> actions))
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
                foreach (ClientKeys clientKey in Enum.GetValues(typeof(ClientKeys)))
                {
                    //if get the clientKey 
                    if (Input.GetKeyDown((KeyCode) clientKey))
                    {
                        //travel all active layer to find pressed key action then invoke it
                        foreach (var dic in KeyCodeDownActiveLayer)
                        {
                            if (dic.TryGetValue(clientKey, out List<Action> actions))
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
        }


        public void RegisterGetKey(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
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
                var dictionary = new Dictionary<ClientKeys, List<Action>>();
                var list = new List<Action>();
                list.Add(action);
                dictionary.Add(keyCode, list);
                RegisteredKeyCodeDic.Add(targetLayer, dictionary);
                //when input layer is first created, add it to active list, which is defualt
                KeyCodeActiveLayer.Add(dictionary);
            }
        }

        public void UnRegisterGetKey(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Remove(action);
                }
            }
        }

        public void RegisterGetKeyDown(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
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
                var dictionary = new Dictionary<ClientKeys, List<Action>>();
                var list = new List<Action>();
                list.Add(action);
                dictionary.Add(keyCode, list);
                RegisteredKeyCodeDic.Add(targetLayer, dictionary);
                //when input layer is first created, add it to active list, which is defualt
                KeyCodeDownActiveLayer.Add(dictionary);
            }
        }

        public void UnRegisterGetKeyDown(ClientKeys keyCode, Action action,
            InputLayer targetLayer = InputLayer.EngagingGame)
        {
            //RegisterGetKey Event
            if (RegisteredKeyCodeDownDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
            {
                if (valueDic.TryGetValue(keyCode, out List<Action> actions))
                {
                    actions.Remove(action);
                }
            }
        }

        //Interruption Method
        public void InterruptDroneOperation(bool isInterrupt)
        {
            this.SendEvent(new DroneOperationListener() {IsInterrupt = isInterrupt});
        }

        public void RequireInterrupt(InputLayer targetLayer)
        {
            //remove this layer dic 
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
            {
                KeyCodeActiveLayer?.Remove(valueDic);
            }
        }

        public void RecoverInterrupt(InputLayer targetLayer)
        {
            //Recovery this layer
            if (RegisteredKeyCodeDic.TryGetValue(targetLayer, out Dictionary<ClientKeys, List<Action>> valueDic))
            {
                if (!KeyCodeActiveLayer.Contains(valueDic))
                {
                    KeyCodeActiveLayer.Add(valueDic);
                }
            }
        }
    }
}