using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:648379a5-6eec-4015-b7ed-2ba711ba5cfb
	public partial class Exitpanel
	{
		public const string Name = "Exitpanel";
		
		[SerializeField]
		public UnityEngine.UI.Text CargoCountText;
		[SerializeField]
		public UnityEngine.UI.Text SpendTimeText;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn;
		[SerializeField]
		public UnityEngine.UI.Button ReMakeBtn;
		
		private ExitpanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CargoCountText = null;
			SpendTimeText = null;
			ExitBtn = null;
			ReMakeBtn = null;
			
			mData = null;
		}
		
		public ExitpanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ExitpanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ExitpanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
