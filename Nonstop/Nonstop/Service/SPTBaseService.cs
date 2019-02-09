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
        static Uri BASE_ADDRESS = new Uri("https://api.spotify.com/v1/");
        public static async Task<E> getAsync(string path, string token)
        {
            // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
            using (HttpClient client = new HttpClient())
            {
                string responseBody = "";
                try
                {
                    client.BaseAddress = BASE_ADDRESS;
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
                return JsonConvert.DeserializeObject<E>(responseBody);
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
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var query = HttpUtility.ParseQueryString(BASE_ADDRESS.Query);
                    foreach (KeyValuePair<string, string> entry in queries)
                    {
                        query.Add(entry.Key, entry.Value);
                    }

                    Uri uri = new Uri(query.ToString());
                    responseBody = await client.GetStringAsync(uri);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                Console.WriteLine(responseBody);
                return JsonConvert.DeserializeObject<E>(responseBody);
            }
        }

    }
}

