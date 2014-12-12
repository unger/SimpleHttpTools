namespace SimpleHttpTools
{
    using System;
    using System.Net;
    using System.Net.Http;

    public interface ISimpleHttpClient
    {
        bool UseXhr { get; set; }

        Uri BaseAddress { get; set; }

        string Get(string requestUri);

        string PostJson(string requestUri, object postData);

        string PostForm(string requestUri, object postData);

        void Get(string requestUri, Action<string, HttpStatusCode, HttpResponseMessage> callback);

        void PostJson(string requestUri, object postData, Action<string, HttpStatusCode, HttpResponseMessage> callback);

        void PostForm(string requestUri, object postData, Action<string, HttpStatusCode, HttpResponseMessage> callback);
    }
}