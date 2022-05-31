using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Soat.AntiGaspi.Api.Tests.Extensions;
public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PostAsync<TIn>(this HttpClient httpClient, string url, TIn data) 
    {
        return httpClient.PostAsync(url,
            new StringContent(
                JsonConvert.SerializeObject(data), 
                Encoding.UTF8, 
                "application/json"));
    }

    public static async Task<T> ReadAsObject<T>(this HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }
}
