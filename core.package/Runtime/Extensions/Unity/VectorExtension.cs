using System.Collections.Generic;
using UnityEngine;

namespace NecroMacro.Core.Extensions
{
	public static class VectorExtension
	{
		public static Vector2 WithX(this Vector2 v, float x)
		{
			return new(x, v.y);
		}

		public static Vector2 WithFlipX(this Vector2 v, bool flip)
		{
			return new(flip ? -v.x : v.x, v.y);
		}

		public static Vector3 WithFlipX(this Vector3 v)
		{
			return new(-v.x, v.y, v.z);
		}

		public static Vector3 WithFlipX(this Vector3 v, bool flip)
		{
			return new(flip ? -v.x : v.x, v.y, v.z);
		}

		public static Vector3 WithFlipY(this Vector3 v)
		{
			return new(v.x, -v.y, v.z);
		}

		public static Vector2 WithY(this Vector2 v, float y)
		{
			return new(v.x, y);
		}

		public static Vector3 WithZ(this Vector2 v, float value)
		{
			return new(v.x, v.y, value);
		}

		public static Vector3 WithX(this Vector3 v, float value)
		{
			return new(value, v.y, v.z);
		}

		public static Vector3 WithY(this Vector3 v, float value)
		{
			return new(v.x, value, v.z);
		}

		public static Vector3 WithZ(this Vector3 v, float value)
		{
			return new(v.x, v.y, value);
		}

		public static Vector3 WithXInc(this Vector3 v, float inc)
		{
			return new(v.x + inc, v.y, v.z);
		}
		
		public static Vector3 WithYInc(this Vector3 v, float inc)
		{
			return new Vector3(v.x, v.y + inc, v.z);
		}
		
		public static Vector3 WithZInc(this Vector3 v, float inc)
		{
			return new(v.x, v.y, v.z + inc);
		}

		public static Vector2 OnlyXY(this Vector3 v)
		{
			return new(v.x, v.y);
		}

		public static Vector2 SwapXY(this Vector2 v, bool swap = true)
		{
			return swap ? new Vector2(v.y, v.x) : v;
		}

		public static Vector3 SwapXY(this Vector3 v, bool swap = true)
		{
			return swap ? new Vector3(v.y, v.x, v.z) : v;
		}


		public static bool ApproximatelyEqual(this Vector2 lhs, Vector2 rhs)
		{
			return Mathf.Approximately(lhs.x, rhs.x)
				&& Mathf.Approximately(lhs.y, rhs.y);
		}

		public static bool ApproximatelyEqual(this Vector3 lhs, Vector3 rhs)
		{
			return Mathf.Approximately(lhs.x, rhs.x)
				&& Mathf.Approximately(lhs.y, rhs.y)
				&& Mathf.Approximately(lhs.z, rhs.z);
		}

		// Работает в 2 раза быстрее sqrMagnitude, возможно, из-за отсутствия приведения к double
		public static bool IsVeryNear(this Vector2 v1, Vector2 v2)
		{
			float dx = v2.x - v1.x;
			float dy = v2.y - v1.y;
			return dx * dx + dy * dy < .0000001f; // число, достаточно маленькое для сравнения координат
		}

		public static bool IsNear(this Vector2 v1, Vector2 v2)
		{
			return (v1 - v2).SqrMagnitude() < .001f;
		}

		public static bool IsNearby(this Vector2 v1, Vector2 v2, float maxDistance)
		{
			return (v1 - v2).SqrMagnitude() < maxDistance * maxDistance;
		}

		public static Vector2Int RoundToInt(this Vector2 vec)
		{
			return new(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
		}

		public static Vector2Int FloorToInt(this Vector2 vec)
		{
			return new(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
		}

		public static Vector2Int CeilToInt(this Vector2 vec)
		{
			return new(Mathf.CeilToInt(vec.x), Mathf.CeilToInt(vec.y));
		}

		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			return new(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
		}

		public static bool IsNan(this Vector2 vec)
		{
			return float.IsNaN(vec.x) || float.IsNaN(vec.y);
		}

		public static float GetDistanceToSegment(this Vector2 vec, Vector2 p1, Vector2 p2)
		{
			float dx = p2.x - p1.x;
			float dy = p2.y - p1.y;

			if (Mathf.Approximately(dx, 0) && Mathf.Approximately(dy, 0)) {
				// It's a point not a line segment.
				// closest = p1;
				dx = vec.x - p1.x;
				dy = vec.y - p1.y;
				return Mathf.Sqrt(dx * dx + dy * dy);
			}

			// Calculate the t that minimizes the distance.
			float t = ((vec.x - p1.x) * dx + (vec.y - p1.y) * dy) / (dx * dx + dy * dy);

			// See if this represents one of the segment's end points or a point in the middle.
			if (t < 0) {
				// closest = new Vector2(p1.x, p1.y);
				dx = vec.x - p1.x;
				dy = vec.y - p1.y;
			} else if (t > 1) {
				// closest = new Vector2(p2.x, p2.y);
				dx = vec.x - p2.x;
				dy = vec.y - p2.y;
			} else {
				var closest = new Vector2(p1.x + t * dx, p1.y + t * dy);
				dx = vec.x - closest.x;
				dy = vec.y - closest.y;
			}

			return Mathf.Sqrt(dx * dx + dy * dy);
		}

		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tx = v.x;
			float ty = v.y;
			v.x = cos * tx - sin * ty;
			v.y = sin * tx + cos * ty;
			return v;
		}

		public static Vector2 Average(this IEnumerable<Vector2> vectorsList)
		{
			var sum = Vector2.zero;
			int count = 0;

			foreach (var vec in vectorsList) {
				sum += vec;
				count++;
			}
			return sum / count;
		}

		public static Vector3 Average(this IEnumerable<Vector3> vectorsList)
		{
			var sum = Vector3.zero;
			int count = 0;

			foreach (var vec in vectorsList) {
				sum += vec;
				count++;
			}
			return sum / count;
		}

		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
		{
			return new(
				Mathf.Clamp(value.x, min.x, max.x),
				Mathf.Clamp(value.y, min.y, max.y),
				Mathf.Clamp(value.z, min.z, max.z)
			);
		}

		public static string ToIntString(this Vector2 v)
		{
			return $"({v.x:#}, {v.y:#})";
		}
	}
}
