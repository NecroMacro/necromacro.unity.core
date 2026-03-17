using System;

namespace NecroMacro.Core
{
	public struct SignalSubscriptionId : IEquatable<SignalSubscriptionId>, ISubscriptionId
	{
		public ISignalBindingId SignalId { get; }
		public object Callback { get; }

		public SignalSubscriptionId(ISignalBindingId signalId, object callback)
		{
			SignalId = signalId;
			Callback = callback;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(SignalId, Callback);
		}

		public override bool Equals(object that)
		{
			if (that is SignalSubscriptionId thatSignal)
			{
				return Equals(thatSignal);
			}

			return false;
		}

		public bool Equals(SignalSubscriptionId that)
		{
			return Equals(SignalId, that.SignalId)
				&& Equals(Callback, that.Callback);
		}

		public static bool operator ==(SignalSubscriptionId left, SignalSubscriptionId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SignalSubscriptionId left, SignalSubscriptionId right)
		{
			return !left.Equals(right);
		}
	}
}