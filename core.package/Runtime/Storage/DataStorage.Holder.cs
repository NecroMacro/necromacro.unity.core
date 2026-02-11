using System;
using System.Collections.Generic;

namespace NecroMacro.Core.Storage
{
	public abstract partial class DataStorage<T> where T : DataStorage<T>, new()
	{
		[Serializable]
		private class Holder
		{
			public DateTimeOffset SinceDate { get; set; } = DateTimeOffset.Now;

			public DateTimeOffset ChangeDate { get; set; }

			public Dictionary<Type, IDataStorageMember> Storage = new();

			public TM Get<TM>() where TM : class, IDataStorageMember, new()
			{
				var memberType = typeof(TM);
				
				if (Storage.TryGetValue(memberType, out var storageMember)) 
					return (TM)storageMember;
				
				var newlyCreatedMember = new TM();
				Storage[memberType] = newlyCreatedMember;
				return newlyCreatedMember;
			}
		}
	}
}