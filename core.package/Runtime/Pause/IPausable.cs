namespace NecroMacro.Core
{
	public interface IPausable
	{
		bool IsPaused { get; }
		void Pause(object requester);
		void Resume(object requester);
	}
}