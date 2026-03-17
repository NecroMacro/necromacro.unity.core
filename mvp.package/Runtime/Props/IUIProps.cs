using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NecroMacro.UI
{
	public interface IUIProps : IEquatable<IUIProps>
	{
		AssetReference Asset { get; }
		Transform Parent { get; set; }
		bool KeepInCacheAfterClose { get; }
		bool NeedPreloadAtStartup { get; }
		bool SingleInstanceInQueue { get; }
		bool CanBeHiddenByOtherElement { get; }
		bool IsDestroyable { get; }
		int InstanceID { get; }
		WidgetType WidgetType { get; }

		void MarkAsDestroyable();
		void Validate();
		IUIProps GetInstance();
	}
}