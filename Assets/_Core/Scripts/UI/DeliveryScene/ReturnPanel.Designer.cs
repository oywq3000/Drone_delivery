using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:7256a7fa-dc09-435a-b74a-b69d5f9fba8c
	public partial class ReturnPanel
	{
		public const string Name = "ReturnPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn2;
		[SerializeField]
		public UnityEngine.UI.Button RemakeBtn;
		[SerializeField]
		public UnityEngine.UI.Button ReturnBtn;
		
		private ReturnPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ExitBtn = null;
			ExitBtn2 = null;
			RemakeBtn = null;
			ReturnBtn = null;
			
			mData = null;
		}
		
		public ReturnPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ReturnPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ReturnPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
