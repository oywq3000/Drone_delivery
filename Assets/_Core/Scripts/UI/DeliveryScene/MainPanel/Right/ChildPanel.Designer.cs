/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class ChildPanel
	{
		[SerializeField] public SetingPanel SetingPanel;

		public void Clear()
		{
			SetingPanel = null;
		}

		public override string ComponentName
		{
			get { return "ChildPanel";}
		}
	}
}
