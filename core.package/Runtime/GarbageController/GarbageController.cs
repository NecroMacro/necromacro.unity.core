using System;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace NecroMacro.Core
{
	public class GarbageController : IStartable
	{
		public void Start()
		{
			#if !UNITY_EDITOR
			GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
			#endif
		}
		
		public void CollectNow()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}