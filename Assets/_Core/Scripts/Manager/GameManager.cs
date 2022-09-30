using System.Collections;
using Drove;
using Drove.Command.Command;
using QFramework;
using QFramework.Example;
using _Core.Scripts;
using UnityEngine;
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
           this.GetModel<IAerialDroneCount>().InitChannel(30,4,5,25);
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