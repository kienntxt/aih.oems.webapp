using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OEMS.Web
{
    public class Utilities
    {
        HttpClient client = new HttpClient();
        public Utilities()
        {
            client = new HttpClient();
        }
        public async Task<string> GetAPI(string base_api, string param)
        {
            try
            {
                client = new HttpClient();
                string url = base_api + param;
                client.BaseAddress = new Uri(url, UriKind.Absolute);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(url);
                var data = await res.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request to API finding error: " + ex.Message);
                return null;
            }
        }
        public async Task<string> GetDataAPI(string base_api, string param)
        {
            try
            {
                client = new HttpClient();
                string url = base_api + param;
                client.BaseAddress = new Uri(url, UriKind.RelativeOrAbsolute);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                var responseData = string.Empty;
                if (res.IsSuccessStatusCode)
                {
                    responseData = await res.Content.ReadAsStringAsync();
                }
                return responseData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request to API finding error: " + ex.Message);
                return null;
            }
        }
        public async Task<string> PostDataAPI(string api, object dataPost, HttpClient httpClient)
        {
            try
            {
                var content = JsonConvert.SerializeObject(dataPost);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                httpClient.BaseAddress = new Uri(api);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpClient.PostAsync(api, byteContent);
                var responseData = await response.Content.ReadAsStringAsync();

                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request post to API finding error: " + ex.Message);
                return null;
            }
        }

    }
}