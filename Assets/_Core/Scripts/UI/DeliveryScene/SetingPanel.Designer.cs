using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:618b7db6-2e89-4fc6-be0e-7c557e13efb4
	public partial class SetingPanel
	{
		public const string Name = "SetingPanel";
		
		[SerializeField]
		public UnityEngine.UI.Slider BGMusicSeting;
		[SerializeField]
		public UnityEngine.UI.Slider SoundEffectSeting;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn;
		[SerializeField]
		public UnityEngine.UI.Toggle InfSwitch;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn1;
		
		private SetingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BGMusicSeting = null;
			SoundEffectSeting = null;
			ExitBtn = null;
			InfSwitch = null;
			ExitBtn1 = null;
			mData = null;
		}
		
		public SetingPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		SetingPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new SetingPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
