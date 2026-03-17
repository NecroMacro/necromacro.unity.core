using DG.Tweening;
using NecroMacro.Extensions;
using UnityEngine;
using UnityEngine.UI;


namespace NecroMacro.UI.Extensions
{
	public class AddressableImage : MonoBehaviour
	{
		public Image loadingImage;
		public Image targetImage;
		public float rotationSpeed = 20;
		public float appearDuration = 0.3f;
		
		private void Start()
		{
			if (targetImage.sprite != null) return;
			targetImage.enabled = false;
			loadingImage.enabled = true;
		}

		private void Update()
		{
			if (loadingImage.enabled)
			{
				loadingImage.transform.Rotate(Vector3.back,
					rotationSpeed * Time.deltaTime,
					Space.Self);
			}
		}

		public void SetTargetImage(Sprite sprite)
		{
			targetImage.sprite = sprite;
			targetImage.enabled = true;

			loadingImage.DOColor(loadingImage.color.WithAlpha(0), appearDuration)
			            .OnComplete(() => loadingImage.enabled = false);
			
			targetImage.color = targetImage.color.WithAlpha(0);
			targetImage.DOColor(targetImage.color.WithAlpha(1), appearDuration);
		}
	}
}