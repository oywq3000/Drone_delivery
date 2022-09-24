/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class Right : UIElement
	{
		private void Awake()
		{
			ReturnBtn.onClick.AddListener(() =>
			{
				UIKit.OpenPanel("ReturnPanel",UILevel.PopUI);
			});
			SetingBtn.onClick.AddListener(() =>
			{
				UIKit.OpenPanel("SetingPanel",UILevel.PopUI);
			});
		}
		protected override void OnBeforeDestroy()
		{
		}
	}
}