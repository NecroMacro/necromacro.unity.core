using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace NecroMacro.Core.Storage
{
    public class JsonSerializationStrategy : ISerializationStrategy
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializationStrategy(JsonSerializerSettings settings = null)
        {
            _settings = settings;
        }

        public UniTask<string> Serialize<T>(T data) =>
            UniTask.FromResult(JsonConvert.SerializeObject(data, _settings));

        public UniTask<T> Deserialize<T>(string raw) =>
            UniTask.FromResult(JsonConvert.DeserializeObject<T>(raw, _settings));
    }
}