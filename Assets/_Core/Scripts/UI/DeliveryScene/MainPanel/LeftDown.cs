/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEngine.Events;

namespace QFramework.Example
{
    public partial class LeftDown : UIElement
    {
        //Set dropdown list
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
            if (str != null)
            {
                var replace = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");
                //plus one indicates a special item
                Dropdown.value = int.Parse(replace) + 1;
            }
            else
            {
                Dropdown.value = 0;
            }
        }

        protected override void OnBeforeDestroy()
        {
            Dropdown.options.Clear();
        }
    }
}