using _Core.Drove.Script.System;
using Drove.Command.Command;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using _Core.Scripts;

namespace QFramework.Example
{
	public class ExitpanelData : UIPanelData
	{
		public int CargoCount;
		public string SpendTime;
	}
	public partial class Exitpanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ExitpanelData ?? new ExitpanelData();
			// please add init code here
			ExitBtn.onClick.AddListener(() =>
			{
				this.SendCommand(new SwitchSceneCmd("Menu"));
				
			});
			
			ReMakeBtn.onClick.AddListener(() =>
			{
		
				this.SendCommand(new SwitchSceneCmd("DeliveryPark"));
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			CargoCountText.text = "送货件数：" + mData.CargoCount;
			SpendTimeText.text = "耗时：" + mData.SpendTime;
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
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
