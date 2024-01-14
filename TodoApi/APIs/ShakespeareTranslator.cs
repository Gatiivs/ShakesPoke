using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace TodoApi.APIs;

public class ShakespeareTranslator
{
    private static readonly HttpClient client = new HttpClient();
    private const string TranslateUrl = "https://api.funtranslations.com/translate/shakespeare.json";

    public static async Task<string> TranslateToShakespearean(string pokeText)
    {
        try
        {
            var builder = new UriBuilder(TranslateUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            Console.WriteLine("pokeText:"+ pokeText);
            query["text"] = pokeText;
            builder.Query = query.ToString();

            string url = builder.ToString();
            Console.WriteLine("Shakespeare url:"+ url);
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                // Handle the case where the request is not successful
                return "Error in Shakespeare translation";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Shakespeare translation: {ex.Message}");
            return "Error in Shakespeare translation";
        }
    }
}
