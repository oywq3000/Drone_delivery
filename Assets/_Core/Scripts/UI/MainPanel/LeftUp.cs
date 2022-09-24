/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.Events;

namespace QFramework.Example
{
	public partial class LeftUp : UIElement
	{
		public void RegisterUltraViewbtn(UnityAction listener)
		{
			UltraViewBtn.onClick.AddListener(listener);
		}

		public void RegisterPilotLineBtn(UnityAction listener)
		{
			PilotLineBtn.onClick.AddListener(listener);
		}

		protected override void OnBeforeDestroy()
		{
		}
	}
}