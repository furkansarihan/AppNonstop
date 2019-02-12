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
    class NonstopBaseService<E> : BaseDispose
    {
        private static string BASE_ADDRESS = "https://app-nonstop.appspot.com/analyse/";

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

                    responseBody = await client.GetStringAsync(path + "/" + token);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return JsonConvert.DeserializeObject<E>(responseBody);
            }
        }
    }
}
