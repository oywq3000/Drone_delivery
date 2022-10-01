using System.Collections.Generic;
using _Core.Drove.Event;
using Cinemachine;
using Drove;
using Drove.Command.Command;
using UnityEngine;
using UnityEngine.UI;
using _Core.Scripts;
using _Core.Scripts.Controller;
namespace QFramework.Example
{
	public class MainPanelData : UIPanelData
	{
		
		
	}
	public partial class MainPanel : UIPanel,IController
	{

		private List<Dropdown.OptionData> _optionDatas;



		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as MainPanelData ?? new MainPanelData();
			// please add init code here
			//initiate LeftDownZone
		}
		
		
		protected override void OnOpen(IUIData uiData = null)
		{
			this.RegisterEvent<OnInfSwitchChanged>(e =>
			{
				RightUp.SetInfTextActive(e.enable);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
			LeftDownInit();
			LeftUpInit();
			RightDownInti();
			RightUpInti();
		}
		
		protected override void OnClose()
		{
		}
		
		#region ForLeftDown

		private void LeftDownInit()
		{
			List<Dropdown.OptionData> Optionlsit = new List<Dropdown.OptionData>();
			//Initiate
			InitOptionList();
			
			//Register a Listener Event
			this.RegisterEvent<DroneListChanged>(e =>
			{
				if (Optionlsit.Count>0)
				{
					
					var optionData = Optionlsit.Find(a => a.text == int.Parse(e.DroneController.Id).ToString() + "号");
					LeftDown.RemoveOption(optionData);
				}
				
			});
			LeftDown.RegistEvent(OnValueChanged);
			Camera.main.GetComponent<CinemachineBrain>().m_CameraActivatedEvent.AddListener((a,b) =>
			{
				//filter event that is not to the DroneCamera
				if (!a.VirtualCameraGameObject.CompareTag("DroneCamera"))
				{
					return;
				}
				//Avoid looping to trigger this two Events
				LeftDown.UnRegisterEvent(OnValueChanged);
				var id = a.VirtualCameraGameObject.transform.parent.GetComponent<DroneController>().Id;
				LeftDown.ChangeCurrentDropDownIndex(id);
				//recovery the Event Register in the end
				RightDown.ChangeCurrentDropDownIndex(null);
				LeftDown.RegistEvent(OnValueChanged);
			
			});

			
			
			#region Inner Function
			void OnValueChanged(int index)
			{
				//Event filter 
				if (index == 0)
				{
					return;
				}
				var replace = System.Text.RegularExpressions.Regex.Replace(LeftDown.GetDropDownList()[index].text, @"[^0-9]+", "");
				this.SendCommand(new SwitchViewCamera(replace));
			}
			
		
			void InitOptionList()
			{
				string specialItem = "无人机";
				//LeftDwonInit
				var droneControllers = this.GetModel<IDroneCountModel>().DroneList;
				for (int i = droneControllers.Count-1; i >= 0; i--)
				{
					var optionData = new Dropdown.OptionData(int.Parse(droneControllers[i].Id).ToString()+"号");
					Optionlsit.Add(optionData);
				}
				Optionlsit.Add(new Dropdown.OptionData(specialItem));
				Optionlsit.Reverse();
				LeftDown.SetDropDrownList(Optionlsit);
			}

			#endregion
			
		}

		
		#endregion
		#region ForLeftUp
		private void LeftUpInit()
		{
			
			LeftUp.RegisterUltraViewbtn(() =>
			{
				this.SendCommand<SetUltraViewCmd>();
			});
			
			LeftUp.RegisterPilotLineBtn(() =>
			{
				this.SendCommand<EnableLightTrick>();
			});
			
		}
		#endregion
		#region ForRightUp

		void RightUpInti()
		{
			//RightUp Event
			RightUp.OnAllCargoHasSent = (cargoCount,spendTime) =>
			{
				UIKit.ClosePanel<MainPanel>();
				UIKit.OpenPanel<Exitpanel>(new ExitpanelData(){CargoCount =cargoCount,SpendTime = spendTime});
			};
		}
		#endregion
		#region RightDown
		void RightDownInti()
		{
			string specialItem = "场景";
			//LeftDwonInit
			var sceneCameraList = this.GetModel<ISceneCameraModel>().SceneCameraList;
			List<Dropdown.OptionData> lsit = new List<Dropdown.OptionData>();
			lsit.Add(new Dropdown.OptionData(specialItem));
			foreach (var VARIABLE in sceneCameraList)
			{
				var optionData = new Dropdown.OptionData(VARIABLE.Id);
				lsit.Add(optionData);
			}
			RightDown.SetDropDrownList(lsit);
			RightDown.RegistEvent(OnValueChanged);
			//Register a Listener Event
			Camera.main.GetComponent<CinemachineBrain>().m_CameraActivatedEvent.AddListener((a,b) =>
			{

				//filter event that is not to the SceneCamera
				if (!a.VirtualCameraGameObject.CompareTag("SceneCamera"))
				{
					return;
				}
				//Avoid looping to trigger this two Events
				RightDown.UnRegisterEvent(OnValueChanged);
				var id = a.VirtualCameraGameObject.GetComponent<SceneCameraController>().Id;
				RightDown.ChangeCurrentDropDownIndex(id);
				//recovery the Event Register in the end
				LeftDown.ChangeCurrentDropDownIndex(null);
				RightDown.RegistEvent(OnValueChanged);
			
			});

			void OnValueChanged(int index)
			{
				if (index == 0)
				{
					return;
				}
				this.SendCommand(new SwitchSceneCamera(RightDown.GetDropDownList()[index].text));
			}

		}
		#endregion
		public IArchitecture GetArchitecture()
		{
			return DroneArchitecture.Interface;
		}
	}
}
