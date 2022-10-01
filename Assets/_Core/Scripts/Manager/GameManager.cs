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
          
            yield return new WaitForEndOfFrame();
           this.SendCommand<InitiateDeliveryScene>();
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

            if (Input.GetKeyDown(KeyCode.T))
            {
                UIKit.ClosePanel<MainPanel>();
                UIKit.OpenPanel<Exitpanel>(new ExitpanelData() {CargoCount = 1, SpendTime = "1"});
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