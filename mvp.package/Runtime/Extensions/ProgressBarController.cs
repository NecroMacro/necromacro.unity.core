using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NecroMacro.Ui
{
	public class ProgressBarController : MonoBehaviour
	{
		[SerializeField]
		private SlicedFilledImage fillImage;

		private CancellationTokenSource animationToken;
		private CancellationToken destroyToken;

		private void Start()
		{
			fillImage.fillAmount = 0;
			destroyToken = destroyCancellationToken;
		}

		public void SetProgress(float progress)
		{
			progress = Mathf.Clamp01(progress);

			animationToken?.Cancel();
			animationToken?.Dispose();

			animationToken = new CancellationTokenSource();

			AnimateProgress(progress, 0.3f, animationToken.Token).Forget();
		}

		private async UniTaskVoid AnimateProgress(
			float targetProgress,
			float duration,
			CancellationToken token)
		{
			float startProgress = fillImage.fillAmount;
			float elapsed = 0f;

			while (elapsed < duration)
			{
				if (token.IsCancellationRequested)
					return;
				
				if (destroyToken.IsCancellationRequested)
					return;

				elapsed += Time.deltaTime;

				float t = Mathf.Clamp01(elapsed / duration);
				fillImage.fillAmount = Mathf.Lerp(startProgress, targetProgress, t);

				await UniTask.NextFrame(token);
			}

			fillImage.fillAmount = targetProgress;
		}
	}
}