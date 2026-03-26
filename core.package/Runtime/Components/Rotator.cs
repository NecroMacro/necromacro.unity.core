using UnityEngine;

namespace NecroMacro.Core.Components
{
	public class Rotator : MonoBehaviour
	{
		public Vector3 rotationAxis = Vector3.up; // ось вращения
		public float speed = 50f; // градусов в секунду

		private void Update()
		{
			transform.rotation *= Quaternion.AngleAxis(speed * Time.deltaTime, rotationAxis.normalized);
		}
	}
}