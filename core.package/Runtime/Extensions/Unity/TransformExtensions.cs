using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public static partial class TransformExtensions
	{
		public static void SetLocalPositionAndRotation(this Transform transform, Vector3 pos, Quaternion rot)
		{
			transform.localPosition = pos;
			transform.localRotation = rot;
		}

		public static void SetPositionX(this Transform transform, float x)
		{
			transform.position = transform.position.WithX(x);
		}

		public static void SetPositionY(this Transform transform, float y)
		{
			transform.position = transform.position.WithX(y);
		}

		public static void SetPositionZ(this Transform transform, float z)
		{
			transform.position = transform.position.WithZ(z);
		}

		public static void SetPositionXY(this Transform transform, Vector2 xy)
		{
			transform.position = xy.WithZ(transform.position.z);
		}

		public static void SetPositionXY(this Transform transform, float x, float y)
		{
			transform.position = new Vector3(x, y, transform.position.z);
		}

		public static void SetLocalPositionX(this Transform transform, float x)
		{
			transform.localPosition = transform.localPosition.WithX(x);
		}

		public static void SetLocalPositionY(this Transform transform, float y)
		{
			transform.localPosition = transform.localPosition.WithY(y);
		}

		public static void SetLocalPositionZ(this Transform transform, float z)
		{
			transform.localPosition = transform.localPosition.WithZ(z);
		}

		public static void SetLocalPositionXY(this Transform transform, Vector2 xy)
		{
			transform.localPosition = xy.WithZ(transform.localPosition.z);
		}

		public static void SetLocalPositionXY(this Transform transform, float x, float y)
		{
			transform.localPosition = new Vector3(x, y, transform.localPosition.z);
		}

		public static void FlipLocalScaleX(this Transform transform, bool flip = true)
		{
			transform.localScale = transform.localScale.WithFlipX(flip);
		}
	}
}
