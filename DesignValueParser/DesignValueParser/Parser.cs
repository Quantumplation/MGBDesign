using Jil;
using System.IO;

namespace DesignValueParser
{
    public class Parser
    {
        public TResult Parse<TResult>(TextReader json)
            where TResult : Result
        {
            return JSON.Deserialize<TResult>(json);
        }
    }
}
