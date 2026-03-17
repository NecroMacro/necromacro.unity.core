using System;
using System.Collections.Generic;

namespace NecroMacro.Core
{
	public class LocksContainer
	{
		private readonly HashSet<object> requesters = new();

		public event Action OnUnlock;
		public event Action OnLock;

		public bool IsLocked => requesters.Count > 0;


		public void Lock(object requester)
		{
			bool wasLocked = IsLocked;
			requesters.Add(requester);
			if (wasLocked || !IsLocked) return;
			OnLock?.Invoke();
		}

		public void Unlock(object requester)
		{
			bool wasLocked = IsLocked;
			requesters.Remove(requester);
			if (!wasLocked || IsLocked) return;
			OnUnlock?.Invoke();
		}

		public void ForceUnlock()
		{
			if (!IsLocked) return;
			requesters.Clear();
			OnUnlock?.Invoke();
		}

		public void Reset()
		{
			requesters.Clear();
			OnLock = null;
			OnUnlock = null;
		}

		#if UNITY_EDITOR
		public HashSet<object> Requesters_Editor => requesters;
		#endif
	}
}