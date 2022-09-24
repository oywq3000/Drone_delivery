/****************************************************************************
 * 2022.9 ADMINSTER
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public partial class RightUp
	{
		[SerializeField] public UnityEngine.UI.Text DroneInf;
		[SerializeField] public UnityEngine.UI.Text CompleteCargo;

		public void Clear()
		{
			DroneInf = null;
			CompleteCargo = null;
		}

		public override string ComponentName
		{
			get { return "RightUp";}
		}
	}
}
