namespace SimpleHttpTools
{
    using System;
    using System.Net;
    using System.Net.Http;

    public class SimpleHttpClient
    {
        private readonly HttpClient httpClient;

        public SimpleHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookies };
            this.httpClient = new HttpClient(handler);
        }

        public string GetStringAsync(Uri requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            return this.RequestStringAsync(request);
        }

        public string PostXhrStringAsync(Uri requestUri, object postData, string contentType)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, contentType);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            return this.RequestStringAsync(request);
        }

        public string PostStringAsync(Uri requestUri, object postData, string contentType)
        {
            var request = this.GetPostRequestMessage(requestUri, postData, contentType);
            return this.RequestStringAsync(request);
        }

        public string RequestStringAsync(HttpRequestMessage request)
        {
            var result = this.httpClient.SendAsync(request).Result;
            result.EnsureSuccessStatusCode();
            return result.Content.ReadAsStringAsync().Result;            
        }

        private HttpRequestMessage GetPostRequestMessage(Uri requestUri, object postData, string contentType)
        {
            var contentFactory = new HttpContentFactory();
            return new HttpRequestMessage(HttpMethod.Post, requestUri)
                              {
                                  Content = contentFactory.Create(postData, contentType)
                              };
        }
    }
}
