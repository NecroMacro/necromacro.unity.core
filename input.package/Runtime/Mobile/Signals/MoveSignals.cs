using UnityEngine;

namespace Core.MobileInput
{
	public class MoveSignal
	{
		public Vector3 Delta { get; }
		
		public MoveSignal(float x, float y) 
		{
			Delta = new Vector3(y, 0, x);
		}
	}
}
