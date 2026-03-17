using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace NecroMacro.UI
{
	public abstract class UIProps : ScriptableObject, IUIProps
	{
		[Header("Basic props settings")]
		[SerializeField]
		private WidgetType widgetType;
		
		[SerializeField]
		private AssetReferenceGameObject asset;

		[SerializeField]
		private bool keepInCacheAfterClose;

		[SerializeField]
		private bool needPreloadAtStartup;

		[Header("Queue")]
		[SerializeField]
		private bool singleInstanceInQueue = true;

		[SerializeField]
		[Tooltip("Widget won't be hidden if another is added to the queue")]
		private bool canBeHiddenByOtherElement = true;
		
		
		public WidgetType WidgetType => widgetType;
		public AssetReference Asset => asset;
		public Transform Parent { get; set; }
		public bool KeepInCacheAfterClose => keepInCacheAfterClose;
		public bool NeedPreloadAtStartup => needPreloadAtStartup;
		public bool SingleInstanceInQueue => singleInstanceInQueue;
		public bool CanBeHiddenByOtherElement => canBeHiddenByOtherElement;
		public bool IsDestroyable { get; private set; }
		public int InstanceID => GetInstanceID();

		public void MarkAsDestroyable()
		{
			IsDestroyable = true;
		}

		public void Validate()
		{
			if (Asset == null) Debug.LogError($"UIProps ({name}) -> Asset is null");
		}

		public IUIProps GetInstance()
		{
			return this;
		}

		public bool Equals(IUIProps other)
		{
			if (ReferenceEquals(other, null))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return InstanceID == other.InstanceID;
		}
		
		public override int GetHashCode()
		{
			return InstanceID;
		}
	}
}