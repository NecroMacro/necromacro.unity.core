using System.Collections.Generic;
using System.Linq;
using NecroMacro.Core.Extensions;
using UnityEngine.AddressableAssets;

namespace NecroMacro.UI
{
	public class WidgetCache
	{
		private readonly Dictionary<IUIProps, List<IWidget>> cache = new();

		public void Add(IUIProps props, IWidget widget)
		{
			if (cache.TryGetValue(props, out var cachedList))
			{
				cachedList.Add(widget);
			}
			else
			{
				var newQueue = new List<IWidget> { widget };
				cache.Add(props, newQueue);
			}
		}

		public IWidget GetAndRemove(IUIProps props)
		{
			var widget = Get(props);
			if (widget == null)
				return null;
			Remove(props, widget);
			return widget;
		}
		
		
		public IWidget Get(IUIProps props)
		{
			var cachedList = cache.GetValueOrDefault(props);
			return cachedList?.Count > 0 ? cachedList[0] : null;
		}

		public IWidget Get(AssetReference reference)
		{
			var key = cache.Keys.FirstOrDefault(e => e.Asset.AssetGUID == reference.AssetGUID);
			return key == null ? null : Get(key);
		}

		public void Remove(IUIProps props, IWidget widget)
		{
			if (cache.TryGetValue(props, out var list))
			{
				list.Remove(widget);
				if (list.IsEmpty())
					cache.Remove(props);
			}
		}

		public void Clear()
		{
			cache.Clear();
		}
	}
}