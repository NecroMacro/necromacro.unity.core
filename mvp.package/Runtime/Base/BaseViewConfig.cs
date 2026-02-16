using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NecroMacro.MVP
{
	public class BaseViewConfig : ScriptableObject
	{
		[SerializeField]
		private AssetReferenceGameObject asset;
		public AssetReferenceGameObject Asset => asset;
	}
}