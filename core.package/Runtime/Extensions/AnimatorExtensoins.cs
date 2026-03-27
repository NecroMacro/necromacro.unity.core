using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public struct AnimationId
	{
		public int Hash;
		public int Layer;
	}
	
	public static class AnimatorExtensions
	{
		public static void RunFadeAnimation(this Animator animator, AnimationId info, float duration = 0.1f)
		{
			var state = animator.GetCurrentAnimatorStateInfo(info.Layer);
			if (state.shortNameHash == info.Hash)
			{
				animator.CrossFade(info.Hash, 0f, info.Layer, 0f);
				return;
			}
			
			animator.CrossFade(info.Hash, duration, info.Layer);
		}	
	}
}