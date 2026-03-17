using System;

namespace NecroMacro.Core
{
	public interface ISignalBindingId
	{
		Type Type { get; }
		object Identifier { get; }
	}
}