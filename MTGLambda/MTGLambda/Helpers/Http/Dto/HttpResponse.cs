using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Http.Dto
{
    public class HttpResponse
    {
        public bool IsSuccessStatusCode { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public HttpResponse() { }

        public HttpResponse(HttpResponseMessage response)
        {
            this.IsSuccessStatusCode = response.IsSuccessStatusCode;
            this.StatusCode = response.StatusCode;
            this.ReasonPhrase = response.ReasonPhrase;

            this.Content = response.Content.ReadAsStringAsync().Result;
        }
    }
}
