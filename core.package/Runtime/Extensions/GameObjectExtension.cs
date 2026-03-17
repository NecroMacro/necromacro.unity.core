using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public static class GameObjectExtension
	{
		public static T GetOrAddComponent<T>(this GameObject target) where T : Component
		{
			if (target == null)
			{
				Debug.LogError($"Trying to add component {typeof(T).Name} to a null game object!");
				return null;
			}
			var component = target.GetComponent<T>();
			if (component == null) component = target.AddComponent<T>();
			return component;
		}
		
		public static void SetLayerRecursively(this Component component, int layer)
		{
			component.gameObject.SetLayerRecursively(layer);
		}

		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;

			foreach (Transform child in gameObject.transform)
			{
				child.gameObject.SetLayerRecursively(layer);
			}
		}
	}
}