
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using QFramework;
using UnityEngine;

namespace Drove.Command.Command
{
    public class SwitchSceneCmd: AbstractCommand
    {
        private string _sceneName;

        public SwitchSceneCmd(string sceneName)
        {
            _sceneName = sceneName;
        }

        protected override void OnExecute()
        {
            //On Scene Exit you need to Release Resource
            Debug.Log("Switch Scene");
            //EndingDelivery
            this.SendCommand<EndingDelivery>();
            this.GetSystem<ISceneSystem>().SwitchScene(_sceneName);
        }
    }
}