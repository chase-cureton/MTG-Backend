using MTGLambda.MTGLambda.Helpers.Http.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Http
{
    public static class HttpHelper
    {
        public static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        public static HttpResponse Post(HttpRequest request)
        {

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(GetTimeout());

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, request.Url);

                SetRequestHeaders(request, httpRequestMessage);

                SetContent(request, httpRequestMessage);

                var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;

                var response = new HttpResponse(httpResponseMessage);

                return response;
            }
        }

        private static void SetContent(HttpRequest request, HttpRequestMessage httpRequestMessage)
        {
            if (request.Headers.ContainsKey("Content-Type"))
            {
                var contentType = request.Headers["Content-Type"];
                var contentTypeParts = contentType.Split(';');
                var mediaType = contentTypeParts[0];

                httpRequestMessage.Content = new StringContent(request.Content, Encoding.UTF8, mediaType);
            }
            else
            {
                httpRequestMessage.Content = new StringContent(request.Content);
            }
        }

        public static HttpResponse Get(HttpRequest request)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(GetTimeout());

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.Url);

                SetRequestHeaders(request, httpRequestMessage);

                var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;

                var response = new HttpResponse(httpResponseMessage);

                return response;
            }
        }

        private static long GetTimeout()
        {
            return 200;
        }

        private static void SetRequestHeaders(HttpRequest request, HttpRequestMessage httpRequest)
        {
            foreach (var header in request.Headers)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
