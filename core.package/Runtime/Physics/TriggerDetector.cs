using UnityEngine;

namespace NecroMacro.Core.Physics
{
	public abstract class TriggerDetector : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			OnEnterTrigger(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnExitTrigger(other);
		}

		protected abstract void OnEnterTrigger(Collider other);
		protected abstract void OnExitTrigger(Collider other);
	}
}