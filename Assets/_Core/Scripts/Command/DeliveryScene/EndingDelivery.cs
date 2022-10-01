
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using QFramework;
namespace Drove.Command.Command
{
    public class EndingDelivery: AbstractCommand
    {
        /// <summary>
        ///Release and Clean Resource
        /// </summary>
        protected override void OnExecute()
        {
            //Release resource
            AudioKit.StopMusic();
            UIKit.CloseAllPanel();
            this.SendEvent<OnSceneExit>();
            this.GetSystem<INavigationSystem>().ReleaseSYstemModel();
        }
    }
}