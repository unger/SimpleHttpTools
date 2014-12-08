namespace SimpleHttpTools
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Web.Script.Serialization;

    public class HttpContentFactory
    {
        public HttpContent Create(object postData, string contentType)
        {
            if (contentType == ContentTypes.FormUrlencoded)
            {
                return this.CreateFormUrlEncodedContent(postData);
            }

            if (contentType == ContentTypes.Json)
            {
                return this.CreateJsonContent(postData);
            }

            return new StringContent(postData.ToString(), Encoding.UTF8, ContentTypes.Text);
        }

        public HttpContent CreateJsonContent(object postData)
        {
            var serializer = new JavaScriptSerializer();
            var content = serializer.Serialize(postData);
            return new StringContent(content, Encoding.UTF8, ContentTypes.Json);
        }

        public FormUrlEncodedContent CreateFormUrlEncodedContent(object postData)
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
