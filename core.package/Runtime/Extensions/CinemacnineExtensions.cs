using Cysharp.Threading.Tasks;
using Unity.Cinemachine;

namespace NecroMacro.Core.Extensions
{
	public static class CinemacnineExtensions
	{
		public static async UniTask WaitForBlending(this CinemachineBrain brain)
		{
			await UniTask.WaitWhile(() => brain.IsBlending);
			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
		}
	}
}