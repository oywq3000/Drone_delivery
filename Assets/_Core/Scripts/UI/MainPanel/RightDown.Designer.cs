/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class RightDown
	{
		[SerializeField] public UnityEngine.UI.Dropdown Dropdown;

		public void Clear()
		{
			Dropdown = null;
		}

		public override string ComponentName
		{
			get { return "RightDown";}
		}
	}
}
