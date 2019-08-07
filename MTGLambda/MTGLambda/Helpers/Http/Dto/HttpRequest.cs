using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Http.Dto
{
    public class HttpRequest
    {
        public string Content { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
