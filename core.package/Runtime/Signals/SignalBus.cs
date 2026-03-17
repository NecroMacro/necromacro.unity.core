using System;
using System.Collections.Generic;

namespace NecroMacro.Core
{
	public class SignalBus : ISignalBus
	{
		private readonly Dictionary<ISignalBindingId, List<Action<object>>> declarationMap = new();
		private readonly Dictionary<ISubscriptionId, Action<object>> subscriptionMap = new();

		public void Reset()
		{
			subscriptionMap.Clear();
			declarationMap.Clear();
		}

		public void Fire<TSignal>(TSignal signal)
		{
			if (signal is ISignalIdentifier { SignalIdentifier: not null } signalWithIdentifier)
				Fire(signal, signalWithIdentifier.SignalIdentifier);
			Fire(signal, null);
		}

		private void Fire<TSignal>(TSignal signal, object identifier)
		{
			var bindingId = new BindingId(signal.GetType(), identifier);
			if (!declarationMap.TryGetValue(bindingId, out var subscribers)) return;
			foreach (var action in subscribers.ToArray())
			{
				action?.Invoke(signal);
			}
		}

		public ISubscriptionId Subscribe<TSignal>(Action<TSignal> callback)
		{
			return SubscribeId(null, callback);
		}

		public ISubscriptionId Subscribe(Type signalType, Action<object> callback)
		{
			return SubscribeId(signalType, null, callback);
		}

		public ISubscriptionId SubscribeId<TSignal>(object identifier, Action<TSignal> callback)
		{
			return SubscribeInternal(
				typeof(TSignal), identifier, callback, args =>
				{
					callback((TSignal)args);
				}
			);
		}

		public ISubscriptionId SubscribeId(Type signalType, object identifier, Action<object> callback)
		{
			return SubscribeInternal(signalType, identifier, callback, callback);
		}

		private ISubscriptionId SubscribeInternal(
			Type signalType, object identifier, object callback, Action<object> wrapperCallback
		)
		{
			return SubscribeInternal(new BindingId(signalType, identifier), callback, wrapperCallback);
		}

		private ISubscriptionId SubscribeInternal(BindingId bindingId, object callback, Action<object> wrapperCallback)
		{
			return SubscribeInternal(new SignalSubscriptionId(bindingId, callback), wrapperCallback);
		}

		private ISubscriptionId SubscribeInternal(SignalSubscriptionId id, Action<object> wrapperCallback)
		{
			if (!subscriptionMap.TryAdd(id, wrapperCallback)) return id;

			if (!declarationMap.ContainsKey(id.SignalId)) declarationMap[id.SignalId] = new List<Action<object>>();

			declarationMap[id.SignalId].Add(wrapperCallback);
			return id;
		}


		public void Unsubscribe<TSignal>(Action<TSignal> callback)
		{
			UnsubscribeId(null, callback);
		}

		public void UnsubscribeId<TSignal>(object identifier, Action<TSignal> callback)
		{
			var bindingId = new BindingId(typeof(TSignal), identifier);
			UnsubscribeInternal(bindingId, callback);
		}

		public void Unsubscribe(ISubscriptionId subscriptionId)
		{
			UnsubscribeInternal(subscriptionId);
		}

		private void UnsubscribeInternal(BindingId signalId, object callback)
		{
			if (callback == null) return;
			var id = new SignalSubscriptionId(signalId, callback);
			UnsubscribeInternal(id);
		}

		private void UnsubscribeInternal(ISubscriptionId subscriptionId)
		{
			if (!subscriptionMap.ContainsKey(subscriptionId) ||
			    !declarationMap.TryGetValue(subscriptionId.SignalId, out var list)) return;

			if (!list.Remove(subscriptionMap[subscriptionId])) return;

			subscriptionMap.Remove(subscriptionId);
			if (list.Count == 0) declarationMap.Remove(subscriptionId.SignalId);
		}
	}
}