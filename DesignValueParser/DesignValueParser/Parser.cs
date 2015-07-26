using System.IO;
using Newtonsoft.Json;

namespace DesignValueParser
{
    public class Parser
    {
        public TResult Parse<TResult>(TextReader json)
            where TResult : Result
        {
            return JsonSerializer.Create().Deserialize<TResult>(new JsonTextReader(json));
        }
    }
}
