using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:835b5ec5-25aa-472f-b443-f478a3bf12c3
	public partial class MainPanel
	{
		public const string Name = "MainPanel";
		
		[SerializeField]
		public LeftUp LeftUp;
		[SerializeField]
		public RightDown RightDown;
		[SerializeField]
		public LeftDown LeftDown;
		[SerializeField]
		public RightUp RightUp;
		[SerializeField]
		public Left Left;
		[SerializeField]
		public Right Right;
		
		private MainPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			LeftUp = null;
			RightDown = null;
			LeftDown = null;
			RightUp = null;
			Left = null;
			Right = null;
			
			mData = null;
		}
		
		public MainPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		MainPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new MainPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
