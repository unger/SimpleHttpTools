namespace SimpleHttpTools
{
    using System;
    using System.Net;
    using System.Net.Http;

    public class SimpleHttpClient : ISimpleHttpClient
    {
        private readonly HttpClient httpClient;

        public SimpleHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookies };
            this.httpClient = new HttpClient(handler);
        }

        public bool UseXhr { get; set; }

        public Uri BaseAddress
        {
            get
            {
                return this.httpClient.BaseAddress;
            }

            set
            {
                this.httpClient.BaseAddress = value;
            }
        }

        public string Get(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            return this.RequestAsString(request);
        }

        public string PostJson(string requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.Json);
            return this.RequestAsString(request);
        }

        public string PostForm(string requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.FormUrlencoded);
            return this.RequestAsString(request);
        }

        private string RequestAsString(HttpRequestMessage request)
        {
            var result = this.httpClient.SendAsync(request).Result;
            result.EnsureSuccessStatusCode();
            return result.Content.ReadAsStringAsync().Result;            
        }

        private HttpRequestMessage GetPostRequestMessage(string requestUri, object postData, string contentType)
        {
            var contentFactory = new HttpContentFactory();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                              {
                                  Content = contentFactory.Create(postData, contentType)
                              };
            if (this.UseXhr)
            {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            return request;
        }
    }
}
