/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class Right
	{
		[SerializeField] public UnityEngine.UI.Button ReturnBtn;
		[SerializeField] public UnityEngine.UI.Button SetingBtn;

		public void Clear()
		{
			ReturnBtn = null;
			SetingBtn = null;
		}

		public override string ComponentName
		{
			get { return "Right";}
		}
	}
}
