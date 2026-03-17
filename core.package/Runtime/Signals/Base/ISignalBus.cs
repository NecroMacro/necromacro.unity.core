using System;

namespace NecroMacro.Core
{
	public interface ISignalBus
	{
		void Reset();

		void Fire<TSignal>(TSignal signal);

		ISubscriptionId Subscribe<TSignal>(Action<TSignal> callback);
		ISubscriptionId Subscribe(Type signalType, Action<object> callback);
		ISubscriptionId SubscribeId(Type signalType, object identifier, Action<object> callback);
		ISubscriptionId SubscribeId<TSignal>(object identifier, Action<TSignal> callback);

		void Unsubscribe<TSignal>(Action<TSignal> callback);
		void Unsubscribe(ISubscriptionId subscriptionId);
	}
}