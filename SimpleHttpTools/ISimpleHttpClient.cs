namespace SimpleHttpTools
{
    using System;

    public interface ISimpleHttpClient
    {
        bool UseXhr { get; set; }

        Uri BaseAddress { get; set; }

        string Get(string requestUri);

        string PostJson(string requestUri, object postData);

        string PostForm(string requestUri, object postData);
    }
}