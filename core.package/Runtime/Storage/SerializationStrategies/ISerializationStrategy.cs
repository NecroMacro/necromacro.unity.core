using Cysharp.Threading.Tasks;

namespace NecroMacro.Core.Storage
{
    public interface ISerializationStrategy
    {
        UniTask<string> Serialize<T>(T data);
        UniTask<T> Deserialize<T>(string raw);
    }
}