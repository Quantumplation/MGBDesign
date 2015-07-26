using System.IO;
using Newtonsoft.Json;

namespace DesignValueParser
{
    public class Parser
    {
        public TResult Parse<TResult>(TextReader json)
        {
            return JsonSerializer.Create().Deserialize<TResult>(new JsonTextReader(json));
        }
    }
}
