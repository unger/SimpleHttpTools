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
            return this.Execute(request);
        }

        public string PostJson(string requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.Json);
            return this.Execute(request);
        }

        public string PostForm(string requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.FormUrlencoded);
            return this.Execute(request);
        }

        public void Get(string requestUri, Action<string, HttpStatusCode, HttpResponseMessage> callback)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            this.Execute(request, callback);
        }

        public void PostJson(string requestUri, object postData, Action<string, HttpStatusCode, HttpResponseMessage> callback)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.Json);
            this.Execute(request, callback);
        }

        public void PostForm(string requestUri, object postData, Action<string, HttpStatusCode, HttpResponseMessage> callback)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, ContentTypes.FormUrlencoded);
            this.Execute(request, callback);
        }

        private async void Execute(HttpRequestMessage request, Action<string, HttpStatusCode, HttpResponseMessage> callback)
        {
            var response = await this.httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: true);
            callback(response.Content.ReadAsStringAsync().Result, response.StatusCode, response);
        }

        private string Execute(HttpRequestMessage request)
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
