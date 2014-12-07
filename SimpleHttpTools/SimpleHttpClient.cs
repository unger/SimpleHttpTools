namespace SimpleHttpTools
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;

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

        public string PostXhrStringAsync(Uri requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            return this.RequestStringAsync(request);
        }

        public string PostStringAsync(Uri requestUri, object postData)
        {
            var request = this.GetPostRequestMessage(requestUri, postData);
            return this.RequestStringAsync(request);
        }

        public string RequestStringAsync(HttpRequestMessage request)
        {
            var result = this.httpClient.SendAsync(request).Result;
            result.EnsureSuccessStatusCode();
            return result.Content.ReadAsStringAsync().Result;            
        }

        private HttpRequestMessage GetPostRequestMessage(Uri requestUri, object postData)
        {
            return new HttpRequestMessage(HttpMethod.Post, requestUri)
                              {
                                  Content = this.ConvertToHttpContext(postData)
                              };
        }

        private FormUrlEncodedContent ConvertToHttpContext(object postData)
        {
            var props = postData.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var nameValuePairs = new List<KeyValuePair<string, string>>();
            foreach (var prop in props)
            {
                nameValuePairs.Add(new KeyValuePair<string, string>(prop.Name, (prop.GetValue(postData) ?? string.Empty).ToString()));
            }

            return new FormUrlEncodedContent(nameValuePairs);
        }
    }
}
