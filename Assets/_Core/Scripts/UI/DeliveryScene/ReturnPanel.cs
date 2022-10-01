using Drove.Command.Command;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using _Core.Scripts;

namespace QFramework.Example
{
	public class ReturnPanelData : UIPanelData
	{
	}
	public partial class ReturnPanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ReturnPanelData ?? new ReturnPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			//ExitBtn Event
		   ExitBtn.onClick.AddListener(() =>
		   {
			   UIKit.ClosePanel<ReturnPanel>();
		   });
		   ExitBtn2.onClick.AddListener(() =>
		   {
			   UIKit.ClosePanel<ReturnPanel>();
		   });
		   //Return  and Remake Event
		   RemakeBtn.onClick.AddListener(() =>
		   {
			   this.SendCommand(new SwitchSceneCmd("DeliveryPark"));
		   });
		   ReturnBtn.onClick.AddListener(() =>
		   {
			   this.SendCommand(new SwitchSceneCmd("Menu"));
		   });
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
