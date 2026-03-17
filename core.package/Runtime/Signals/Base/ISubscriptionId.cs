namespace NecroMacro.Core
{
	public interface ISubscriptionId
	{
		ISignalBindingId SignalId { get; }
		object Callback { get; }
	}
}