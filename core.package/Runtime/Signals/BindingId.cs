using System;

namespace NecroMacro.Core
{
	public struct BindingId : IEquatable<BindingId>, ISignalBindingId
	{
		public Type Type { get; }

		public object Identifier { get; }

		public BindingId(Type type, object identifier)
		{
			Type = type;
			Identifier = identifier;
		}

		public override string ToString()
		{
			return Identifier == null ? Type.ToString() : $"{Type} (ID: {Identifier})";
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Identifier);
		}

		public override bool Equals(object other)
		{
			if (other is not BindingId otherId) return false;
			return otherId == this;

		}

		public bool Equals(BindingId that)
		{
			return this == that;
		}

		public static bool operator ==(BindingId left, BindingId right)
		{
			return left.Type == right.Type && Equals(left.Identifier, right.Identifier);
		}

		public static bool operator !=(BindingId left, BindingId right)
		{
			return !left.Equals(right);
		}
	}
}