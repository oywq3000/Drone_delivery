using System;
using System.Collections;
using _Core.Drove.Event;
using _Core.Drove.Script.System;
using Drove;
using Drove.Command.Command;
using QFramework;
using QFramework.Example;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;
using KeyCode = UnityEngine.KeyCode;

namespace _Core.Drove.Script.Manager
{
    public class GameManager : MonoBehaviour, IController
    {
        private bool switchHolder = true;

        IEnumerator Start()
        {
            ResKit.Init();
            yield return new WaitForEndOfFrame();
            //intiate the FlightChannel
            this.SendCommand(new InitiateFlightChannel(150, 5, 50));
            UIKit.OpenPanel<EntryPanel>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OnOffMainPanel();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                //On Exit you need to Close all the Panel prevent Panel resource destroyed by Monobehavior
                this.SendCommand(new SwitchSceneCmd("DeliveryPark"));
            }
        }

        void OnOffMainPanel()
        {
            if (switchHolder)
            {
                UIKit.HidePanel<MainPanel>();
            }
            else
            {
                UIKit.OpenPanel<MainPanel>();
            }

            switchHolder = !switchHolder;
        }


        void InitiateModel()
        {
            //Initiate Flight Channel every loading Scene
        }

        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}