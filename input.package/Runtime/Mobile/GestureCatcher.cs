using UnityEngine;
using VContainer;

namespace Core.MobileInput
{
	public class GestureCatcher : MonoBehaviour
	{
		[Inject] private readonly GestureController gestureController;
		
		private void OnEnable()
		{
			gestureController.RegisterGestureCatcher(this);
		}

		private void OnDisable()
		{
			gestureController.UnregisterGestureCatcher(this);
		}
	}
}