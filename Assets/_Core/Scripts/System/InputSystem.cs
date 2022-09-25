
using System;
using System.Collections.Generic;
using _Core.Drove.Event;
using QFramework;
using UnityEngine;

namespace _Core.Drove.Script.System
{
    public  enum ClientKeys
    {
        Mouse0 = KeyCode.Mouse0,
        Mouse1 = KeyCode.Mouse1,
    }
    public interface IInputSystem:ISystem
    {

        Dictionary<ClientKeys, List<Action>> KeyQueryDic { get; }
        Dictionary<ClientKeys, List<Action>> KeyQueryDicDown { get; }
        void RegisterGetKey(ClientKeys keyCode, Action action);
        void UnRegisterGetKey(ClientKeys keyCode, Action action);
        void RegisterGetKeyDown(ClientKeys keyCode, Action action);
        void UnRegisterGetKeyDown(ClientKeys keyCode, Action action);
        void InterruptDroneOperation(bool isInterrupt);
    }


   
    public class InputSystem: AbstractSystem,IInputSystem
    {
        /// <summary>
        /// client customize the input buttion
        /// </summary>
      
        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public Dictionary<ClientKeys, List<Action>> KeyQueryDic { get; } = 
            new Dictionary<ClientKeys, List<Action>>();

        public Dictionary<ClientKeys, List<Action>> KeyQueryDicDown { get; } =
            new Dictionary<ClientKeys, List<Action>>();
        
        protected override void OnInit()
        {

           
        }

        public void RegisterGetKey(ClientKeys keyCode, Action action)
        {
            //RegisterGetKey Event
            if ( KeyQueryDic.TryGetValue(keyCode,out List<Action> actions))
            {
               actions.Add(action);
            }
            else 
            {
                var list = new List<Action>();
                list.Add(action);
                KeyQueryDic.Add(keyCode,list);
            }
        }

        public void UnRegisterGetKey(ClientKeys keyCode, Action action)
        {
            //RegisterGetKey Event
            if ( KeyQueryDic.TryGetValue(keyCode,out List<Action> actions))
            {
                actions.Remove(action);
            }
        }
        
        public void RegisterGetKeyDown(ClientKeys keyCode, Action action)
        {
            //RegisterGetKey Event
            if ( KeyQueryDicDown.TryGetValue(keyCode,out List<Action> actions))
            {
                actions.Add(action);
            }
            else 
            {
                var list = new List<Action>();
                list.Add(action);
                KeyQueryDicDown.Add(keyCode,list);
            }
        }

        public void UnRegisterGetKeyDown(ClientKeys keyCode, Action action)
        {
            //RegisterGetKey Event
            if ( KeyQueryDicDown.TryGetValue(keyCode,out List<Action> actions))
            {
                actions.Remove(action);
            }
        }
        
        
        //Interruption Method
      

        public void InterruptDroneOperation(bool isInterrupt)
        {
            this.SendEvent(new DroneOperationListener(){IsInterrupt = isInterrupt});
        }
    }
}