using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Extensions
{
	public static class TaskExtension
	{
		public delegate UniTaskVoid UniTaskVoidAction();
		
		public static async UniTask<T> ContinueWith<T>(
			this UniTask<T> task, Action<T> onComplete, Action<Exception> onException = null
		)
		{
			var result = default(T);
			try {
				result = await task;
				onComplete?.Invoke(result);
			} catch (Exception e) {
				onException?.Invoke(e);
			}
			return result;
		}

		public static void HandleException<T>(this UniTask<T> task, Action<Exception> onException)
		{
			HandleExceptionInternal(task, onException).Forget();
		}

		private static async UniTask<T> HandleExceptionInternal<T>(this UniTask<T> task, Action<Exception> onException)
		{
			var result = default(T);
			try {
				result = await task;
			} catch (Exception e) {
				onException?.Invoke(e);
			}
			return result;
		}

		public static void HandleException(this UniTask task, Action<Exception> onException)
		{
			HandleExceptionInternal(task, onException).Forget();
		}

		private static async UniTask HandleExceptionInternal(this UniTask task, Action<Exception> onException)
		{
			try {
				await task;
			} catch (Exception e) {
				onException?.Invoke(e);
			}
		}

		public static UniTask WhenAll<T>(this IEnumerable<T> items, Func<T, UniTask> action)
		{
			return UniTask.WhenAll(Enumerable.Select(items, action));
		}
	}
}
