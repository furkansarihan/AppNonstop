using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nonstop.Forms.Service
{
    class SPTBaseService<E> : BaseDispose
    {
        private static string BASE_ADDRESS = "https://api.spotify.com/v1/";

        public static async Task<E> getAsync(string path, string token)
        {
            // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
            using (HttpClient client = new HttpClient())
            {
                string responseBody = "";
                try
                {
                    Uri uri = new Uri(BASE_ADDRESS);
                    client.BaseAddress = uri;
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    responseBody = await client.GetStringAsync(path);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                // Some of fields can be null, like image object's width. In this case, just ignore it.
                return JsonConvert.DeserializeObject<E>(responseBody, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }

        public static async Task<E> getAsync(string path, Dictionary<string, string> queries, string token)
        {
            //ref : https://stackoverflow.com/a/17096289/5929406
            using (HttpClient client = new HttpClient())
            {
                string responseBody = "";
                try
                {
                    UriBuilder baseUri = new UriBuilder(BASE_ADDRESS);
                    client.BaseAddress = baseUri.Uri;
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var query = HttpUtility.ParseQueryString(baseUri.Query);
                    foreach (KeyValuePair<string, string> entry in queries)
                    {
                        query.Add(entry.Key, entry.Value);
                    }
                    baseUri.Path += path;
                    baseUri.Query = query.ToString();
                    Uri uri = new Uri(baseUri.ToString());
                    responseBody = await client.GetStringAsync(uri);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return JsonConvert.DeserializeObject<E>(responseBody, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }

    }
}

