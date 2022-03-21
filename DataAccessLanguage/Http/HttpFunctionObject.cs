using System.Collections.Generic;
using System.Net.Http;

namespace DataAccessLanguage.Http
{
    public class HttpFunctionObject
    {
        public HttpClient Http { get; set; }
        public ICollection<KeyValuePair<string, object>> Headers { get; set; } = new List<KeyValuePair<string, object>>();
        public object DataObject { get; set; }
        public string Url { get; set; }
    }
}