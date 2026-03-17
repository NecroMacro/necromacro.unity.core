namespace NecroMacro.Core
{
	public interface ISignalBusesService
	{
		ISignalBus GetBus(string id);
		ISignalBus Default { get; }
	}
}