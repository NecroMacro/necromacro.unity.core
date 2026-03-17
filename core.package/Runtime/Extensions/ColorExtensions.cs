using UnityEngine;

namespace NecroMacro.Extensions
{
	public static class ColorExtensions
	{
		public static Color WithAlpha(this Color source, float alpha) 
		{
			return new Color(source.r, source.g, source.b, alpha);
		}
	}
}