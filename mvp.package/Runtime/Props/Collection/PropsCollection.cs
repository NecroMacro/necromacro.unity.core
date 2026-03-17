using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NecroMacro.UI
{
	public class PropsCollection : IPropsCollection
	{
		private readonly Dictionary<string, IUIProps> propsCollection = new();
		private readonly Dictionary<Type, IUIProps> cachedProps = new();

		public PropsCollection()
		{
		}

		public void InitProps(IEnumerable<IUIProps> propsList)
		{
			if (propsList == null) return;

			foreach (var props in propsList)
			{
				string type = props.GetType().ToString();
				if (string.IsNullOrEmpty(type)) return;
				propsCollection.TryAdd(type, props);
			}
		}

		public List<IUIProps> GetAllPropsReferences()
		{
			return propsCollection.Values.ToList();
		}

		public T GetProps<T>() where T : class, IUIProps
		{
			return GetPropsByType<T>(typeof(T));
		}

		public T GetProps<T>(Type derivedType) where T : class, IUIProps
		{
			var type = typeof(T);
			if (derivedType.IsAssignableFrom(type))
			{
				Debug.LogError($"{derivedType} is not assignable from {type}");
				return null;
			}

			return GetPropsByType<T>(derivedType);
		}

		private T GetPropsByType<T>(Type type) where T : class, IUIProps
		{
			if (propsCollection == null)
			{
				Debug.LogError("PropsCollection -> collection is not initialized.");
				return null;
			}

			var cachedProps = this.cachedProps.GetValueOrDefault(type);
			if (cachedProps != null) return cachedProps as T;

			string id = type.ToString();
			var reference = GetPropsReferenceById(id);
			if (reference == null) return null;

			var props = reference.GetInstance();
			if (props is not T typedProps)
			{
				Debug.LogError($"The type of props does not match the required: {type}");
				return null;
			}

			if (props.KeepInCacheAfterClose) this.cachedProps.Add(type, props);
			else props.MarkAsDestroyable();

			return typedProps;
		}

		private IUIProps GetPropsReferenceById(string id)
		{
			propsCollection.TryGetValue(id, out var reference);
			if (reference != null) return reference;
			Debug.LogError("PropsCollection -> no value found for key: " + id);
			return null;
		}

		void IPropsCollection.Reset()
		{
			propsCollection.Clear();
		}
	}
}