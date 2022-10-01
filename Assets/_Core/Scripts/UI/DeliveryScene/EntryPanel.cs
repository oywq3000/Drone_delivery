using System;
using System.Collections;
using Drove.Command.Command;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using _Core.Scripts;

namespace QFramework.Example
{
    public class EntryPanelData : UIPanelData
    {
    }

    public partial class EntryPanel : UIPanel, IController
    {
        private Coroutine _coroutineHoder;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as EntryPanelData ?? new EntryPanelData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        private void FixedUpdate()
        {
            if (Input.anyKeyDown)
            {
                //Send Start Event
                this.SendCommand<StartDelivery>();
                //SwitchPanel
                UIKit.OpenPanel("MainPanel");
                UIKit.ClosePanel<EntryPanel>();
                AudioKit.PlayMusic("Lost touch");
            }
        }


        protected override void OnClose()
        {
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}