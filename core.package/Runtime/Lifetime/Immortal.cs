using UnityEngine;

namespace NecroMacro.Core.Lifetime
{
	public class Immortal : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}