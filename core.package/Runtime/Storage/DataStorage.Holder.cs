using System;
using System.Collections.Generic;

namespace NecroMacro.Core.Storage
{
	public abstract partial class DataStorage<T> where T : DataStorage<T>, new()
	{
		[Serializable]
		private class Holder
		{
			public DateTimeOffset ChangeDate { get; set; }

			public Dictionary<Type, IDataStorageMember> Storage = new();

			public void Get<TMember>(out TMember member) where TMember : struct, IDataStorageMember
			{
				var memberType = typeof(TMember);

				if (Storage.TryGetValue(memberType, out var storageMember))
				{
					member = (TMember)storageMember;
				}
				else
				{
					member = default;
					Storage[memberType] = member;
				}
			}

			public void Set<TMember>(TMember member) where TMember : struct, IDataStorageMember
			{
				var memberType = typeof(TMember);
				Storage[memberType] = member;
			}
		}
	}
}