using System;
using System.Collections.Generic;

namespace Alexw.StubServer.Core
{
    public class RecordedRequest
    {
        public string Body { get; set; }
        public Dictionary<string, string[]> Headers { get; set; }
        public string ContentType { get; set; }
        public string Scheme { get; set; }
        public string RemoteIpAddress { get; set; }
        public int? RemotePort { get; set; }
        public Uri Uri { get; set; }
    }
}