using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace NecroMacro.Core.Extensions
{
	public static class ListExtension
	{
		public static void ForEach<T>(this IList<T> list, Action<T, int> action)
		{
			for (int i = 0; i < list.Count; i++) {
				action(list[i], i);
			}
		}

		public static void ForEach<T>(this IReadOnlyList<T> list, Action<T, int> action)
		{
			for (int i = 0; i < list.Count; i++) {
				action(list[i], i);
			}
		}

		public static bool ForEachAny<T>(this IReadOnlyList<T> list, Func<T, bool> action)
		{
			if (list == null) {
				return false;
			}
			bool isAny = false;

			for (int i = 0; i < list.Count; i++) {
				isAny |= action(list[i]);
			}
			return isAny;
		}

		[ContractAnnotation("list:null => false")]
		public static bool IsFilled<T>(this List<T> list)
		{
			return list != null && list.Count > 0;
		}

		[ContractAnnotation("list:null => true")]
		public static bool IsEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			if (list == null) {
				return;
			}

			for (int i = 0; i < list.Count; i++) {
				var temp = list[i];
				int randomIndex = Random.Range(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		public static T GetRandom<T>(this IList<T> list)
		{
			var wanted = default(T);
			if (list == null) {
				return wanted;
			}

			int count = list.Count;
			if (count == 0) {
				return wanted;
			}

			int i = Random.Range(0, count);
			return list[i];
		}

		public static void AddIfNewAndNotNull<T>(this IList<T> list, T value)
		{
			if (!typeof(T).IsValueType && Equals(value, null)) {
				return;
			}

			if (list == null) {
				return;
			}

			if (!list.Contains(value)) {
				list.Add(value);
			}
		}

		public static void AddRangeNewNotNull<T>(this IList<T> list, IEnumerable<T> collection)
		{
			if (list == null) {
				return;
			}

			foreach (var element in collection) {
				list.AddIfNewAndNotNull(element);
			}
		}

		public static List<T> CloneList<T>(this IList<T> list) where T : ICloneable
		{
			if (list == null) {
				return null;
			}

			return list.Select(item => item == null ? default : (T)item.Clone()).ToList();
		}

		public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++) {
				if (predicate(list[i])) {
					list.RemoveAt(i);
					i--;
				}
			}
		}

		public static T GetAndRemove<T>(this IList<T> list, int index)
		{
			var value = list[index];
			list.RemoveAt(index);
			return value;
		}

		public static void MoveToEnd<T>(this IList<T> list, int index)
		{
			var value = list.GetAndRemove(index);
			list.Add(value);
		}

		public static bool IndexOutOfBounds<T>(this IList<T> list, int index)
		{
			return list == null || index < 0 || index > list.Count - 1;
		}

		public static bool IsIndexInBounds<T>(this IList<T> list, int index)
		{
			return !list.IndexOutOfBounds(index);
		}

		public static bool UnorderedEqualTo<T>(this IList<T> seq1, IList<T> seq2)
		{
			if (seq1 == null && seq2 == null) {
				return true;
			}

			if (seq1 == null || seq2 == null) {
				return false;
			}

			return seq1.Count == seq2.Count && seq1.All(seq2.Contains);
		}

		public static T GetAtOrDefault<T>(this IReadOnlyList<T> list, int index)
		{
			if (list == null || index >= list.Count || index < 0) {
				return default;
			}
			return list[index];
		}
	}
}
