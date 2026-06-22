using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NecroMacro.Core.Storage
{
	public delegate void DataStorageMemberProcessor<TMember>(ref TMember data)
		where TMember : struct, IDataStorageMember;

	public abstract partial class DataStorage<T> where T : DataStorage<T>, new()
	{
		private bool IsInitialized => holder != null;

		private IStorageDriver storage;
		private ISaveStrategy saveStrategy;
		private ISerializationStrategy serializer;

		private readonly SemaphoreSlim saveSemaphore = new(1, 1);

		private Holder holder;
		private bool pendingDirty;

		protected async UniTask Initialize(
			IStorageDriver driver, ISaveStrategy saveStrategy, ISerializationStrategy serializer
		)
		{
			if (IsInitialized) return;

			storage = driver;
			this.saveStrategy = saveStrategy;
			this.serializer = serializer;

			holder = await TryLoadHolder();
			saveStrategy.Initialize(SaveAsync);
		}

		public UniTask Save()
		{
			return saveStrategy.SaveForce();
		}

		public void Get<TMember>(out TMember member) where TMember : struct, IDataStorageMember
		{
			holder.Get(out member);
		}

		public void Change<TMember>(DataStorageMemberProcessor<TMember> changeAction)
			where TMember : struct, IDataStorageMember
		{
			if (!IsInitialized)
				return;

			ChangeHolder(Action);
			return;

			void Action()
			{
				Get<TMember>(out var member);
				changeAction(ref member);
				holder.Set(member);
			}
		}

		public void Reset()
		{
			if (!IsInitialized)
				return;

			ChangeHolder(() => holder = new Holder());
		}

		private async UniTask<Holder> TryLoadHolder()
		{
			var result = new Holder();

			if (!await storage.Exists())
				return result;

			try
			{
				var rawData = await storage.Read();
				var deserialized = await serializer.Deserialize<Holder>(rawData);
				return deserialized ?? result;
			} catch
			{
				Debug.LogError($"{typeof(T).Name} storage corrupted.");
				return result;
			}
		}

		private async UniTask SaveAsync()
		{
			if (!saveSemaphore.Wait(0))
			{
				pendingDirty = true;
				return;
			}

			try
			{
				do
				{
					pendingDirty = false;
					holder.ChangeDate = DateTimeOffset.Now;

					var data = await serializer.Serialize(holder);
					await storage.Write(data);
				}
				while (pendingDirty);
			} finally
			{
				saveSemaphore.Release();
			}
		}

		private void ChangeHolder(Action processor)
		{
			processor();
			saveStrategy.RequestSave();
		}
	}
}