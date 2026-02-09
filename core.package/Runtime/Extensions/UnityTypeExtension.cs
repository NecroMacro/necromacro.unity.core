using System;
using UnityEngine;

namespace NecroMacro.Core.Extension
{
	using Object = UnityEngine.Object;

	public static class UnityTypeExtension
	{
		private static readonly Type ComponentType = typeof(Component);
		private static readonly Type GameObjectType = typeof(GameObject);
		private static readonly Type AssetType = typeof(Object);
		private static readonly Type ScriptableType = typeof(ScriptableObject);

		public static bool IsComponent(this Type type)
		{
			return type != null && ComponentType.IsAssignableFrom(type);
		}

		public static bool IsGameObject(this Type type)
		{
			return type != null && GameObjectType == type;
		}

		public static bool IsScriptableObject(this Type type)
		{
			return type != null && ScriptableType.IsAssignableFrom(type);
		}

		public static bool IsAsset(this Type type)
		{
			return type != null && AssetType.IsAssignableFrom(type);
		}
	}
}