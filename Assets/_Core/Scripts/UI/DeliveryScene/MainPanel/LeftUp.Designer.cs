/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class LeftUp
	{
		[SerializeField] public UnityEngine.UI.Button UltraViewBtn;
		[SerializeField] public UnityEngine.UI.Button PilotLineBtn;

		public void Clear()
		{
			UltraViewBtn = null;
			PilotLineBtn = null;
		}

		public override string ComponentName
		{
			get { return "LeftUp";}
		}
	}
}
