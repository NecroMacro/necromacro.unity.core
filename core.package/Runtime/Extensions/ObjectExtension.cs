using JetBrains.Annotations;
using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public static class ObjectExtension
	{
		[ContractAnnotation("obj:null => true")]
		public static bool IsNull(this Object obj)
		{
			return ReferenceEquals(obj, null);
		}

		[ContractAnnotation("obj:null => false")]
		public static bool IsNotNull(this Object obj)
		{
			return !obj.IsNull();
		}
	}
}
