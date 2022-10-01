/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.Events;

namespace QFramework.Example
{


	
	public partial class RightDown : UIElement
	{
		public void SetDropDrownList(List<Dropdown.OptionData> optionDatas)
		{
			Dropdown.options = optionDatas;
		}
		public void RegistEvent(UnityAction<int> onValueChange)
		{
			Dropdown.onValueChanged.AddListener(onValueChange);
		}

		public void UnRegisterEvent(UnityAction<int> onValueChange)
		{
			Dropdown.onValueChanged.RemoveListener(onValueChange);
		}
		public List<Dropdown.OptionData> GetDropDownList()
		{
			return Dropdown.options;
		}

		public void ChangeCurrentDropDownIndex(string str)
		{
			if (str!=null)
			{
				//plus one indicates a special item
				Dropdown.value = Dropdown.options.FindIndex(e => e.text == str);
			}
			else
			{
				Dropdown.value = 0;
			}
		}

		
		
		protected override void OnBeforeDestroy()
		{
		}
	}
}