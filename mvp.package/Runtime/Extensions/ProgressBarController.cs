using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Components
{
	public class ProgressBarController : MonoBehaviour
	{
		[SerializeField]
		private Image fillImage;
		
		private CancellationTokenSource animationToken;
		
		private float targetProgress;

		private void Start()
		{
			fillImage.fillAmount = 0;
		}

		public void SetProgress(float progress)
		{
			targetProgress = progress;
			
			animationToken?.Cancel();
			animationToken = new CancellationTokenSource();
			
			UpdateValue(0.3f, animationToken).Forget();
		}
		
		private async UniTaskVoid UpdateValue(float timeSec, CancellationTokenSource token)
		{
			float framesCount = timeSec / Time.deltaTime;
			float oneTickValue = (targetProgress - fillImage.fillAmount) / framesCount;
			
			while (fillImage.fillAmount < targetProgress)
			{
				if (token.IsCancellationRequested) return;
				fillImage.fillAmount += oneTickValue;
				await UniTask.NextFrame();
			}
		}
	}
}