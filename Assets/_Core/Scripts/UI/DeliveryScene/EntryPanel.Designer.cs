using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:40a97513-c0a7-467b-8560-fa5d880cca5e
	public partial class EntryPanel
	{
		public const string Name = "EntryPanel";
		
		
		private EntryPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public EntryPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		EntryPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new EntryPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
