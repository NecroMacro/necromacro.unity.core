using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public static class CameraExtensions
	{
		public static void EnableSkyBox(this Camera camera)
		{
			camera.clearFlags = CameraClearFlags.Skybox;
		}
		
		public static void EnableSolidColor(this Camera camera)
		{
			camera.clearFlags = CameraClearFlags.Color;
		}
	}
}