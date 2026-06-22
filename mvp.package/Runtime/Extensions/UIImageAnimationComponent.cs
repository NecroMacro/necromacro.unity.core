using UnityEngine;
using UnityEngine.UI;

namespace NecroMacro.MVP
{
	public class UIImageAnimationComponent : MonoBehaviour
	{
		public Sprite[] sprites;
		public int spritePerFrame = 6;
		public bool loop = true;
		public bool destroyOnEnd = false;

		private int index = 0;
		private Image image;
		private int frame = 0;

		private void Awake()
		{
			image = GetComponent<Image>();
		}

		private void Update()
		{
			if (!image.enabled)
			{
				frame = 0;
				index = 0;
				return;
			}

			if (!loop && index == sprites.Length) return;
			frame++;
			if (frame < spritePerFrame) return;

			image.sprite = sprites[index];
			frame = 0;
			index++;

			if (index < sprites.Length) return;
			if (loop) index = 0;
			if (destroyOnEnd) Destroy(gameObject);
		}
	}
}