using System.Net.Http.Headers;

namespace VivesRental.Sdk.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddAuthorization(this HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.AddAuthorization(token);
            return httpClient;
        }

        public static HttpRequestHeaders AddAuthorization(this HttpRequestHeaders headers, string token)
        {
            if (headers.Contains("Authorization"))
            {
                headers.Remove("Authorization");
            }

            headers.Add("Authorization", $"Bearer {token}");

            return headers;
        }
    }
}