using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NecroMacro.Core.Extensions;
using UnityEngine;
// ReSharper disable MethodHasAsyncOverload

namespace NecroMacro.Core.Storage
{
	public delegate void DataStorageMemberProcessor<TM>(ref TM data) where TM : class, IDataStorageMember, new();

	public abstract partial class DataStorage<T> where T : DataStorage<T>, new()
	{
		private bool IsInitialized => holder != null;
		
		private IDataStorageBackend backend;
		private ISaveStrategy saveStrategy;

		private readonly SemaphoreSlim saveSemaphore = new(1, 1);
		
		private Holder holder;
		private bool pendingDirty;
		
		protected async UniTask Initialize(IDataStorageBackend backend, ISaveStrategy saveStrategy)
		{
			if (IsInitialized) return;
			
			this.backend = backend;
			this.saveStrategy = saveStrategy;
			
			holder = await TryLoadHolder();
			saveStrategy.Initialize(SaveAsync);
		}
		
		private async UniTask<Holder> TryLoadHolder()
		{
			var result = new Holder();

			if (!await backend.Exists())
				return result;

			try
			{
				string stringData = await backend.Read();
				var deserialized = await stringData.DeserializeJson<Holder>();
				return deserialized ?? result;
			}
			catch
			{
				Debug.LogWarning($"{typeof(T).Name} storage corrupted.");
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
					
					string data = await holder.SerializeJson();
					await backend.Write(data);
				}
				while (pendingDirty);
			}
			finally
			{
				saveSemaphore.Release();
			}
		}

		private void ChangeHolder(Action processor, Type _)
		{
			processor();
			saveStrategy.MarkDirty();
		}
		
		public TMember Get<TMember>() where TMember : class, IDataStorageMember, new()
		{
			return holder.Get<TMember>();
		}
		
		public void Change<TMember>(DataStorageMemberProcessor<TMember> changeAction)
			where TMember : class, IDataStorageMember, new()
		{
			if (!IsInitialized)
				return;
			
			ChangeHolder(Action, typeof(TMember));
			return;

			void Action()
			{
				var storageMember = Get<TMember>();
				changeAction(ref storageMember);
			}
		}
		
		public void Reset()
		{
			ChangeHolder(() => holder = new Holder(), null);
		}
	}
}