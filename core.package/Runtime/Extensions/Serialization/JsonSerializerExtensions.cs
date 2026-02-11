using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace NecroMacro.Core.Extensions
{
	public static class JsonSerializerExtensions
	{
		private static readonly JsonSerializerSettings DefaultSerializerSettings = new()
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting = Formatting.Indented,
			DefaultValueHandling = DefaultValueHandling.Include
		};

		public static UniTask<T> DeserializeJson<T>(this string data, JsonSerializerSettings settings = null)
		{
			return UniTask.RunOnThreadPool(() =>
				{
					using var reader = new StringReader(data);
					using var jsonReader = new JsonTextReader(reader);

					var serializer = JsonSerializer.Create(settings ?? DefaultSerializerSettings);

					return serializer.Deserialize<T>(jsonReader);
				}
			);
		}

		public static UniTask<string> SerializeJson<T>(this T data, JsonSerializerSettings settings = null)
		{
			return UniTask.RunOnThreadPool(() =>
				{
					using var writer = new StringWriter();
					using var jsonWriter = new JsonTextWriter(writer);

					var serializer = JsonSerializer.Create(settings ?? DefaultSerializerSettings);
					serializer.Serialize(jsonWriter, data);
					jsonWriter.Flush();

					return writer.ToString();
				}
			);
		}
	}
}