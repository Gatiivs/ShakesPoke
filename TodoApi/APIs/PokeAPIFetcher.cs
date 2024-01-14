using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoApi.APIs;

public class PokeAPIFetcher
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "https://pokeapi.co/api/v2/pokemon-species/";

    public static async Task<string> GetPokemonInfo(string pokemonName)
    {
        try
        {
            string url = $"{BaseUrl}{pokemonName}";
            Console.WriteLine(url);
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
              //  Console.WriteLine("pokeAPIresult"+ result);
                return result;
            }
            else
            {
                // Handle the case where the request is not successful
                return "request unsucessful";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Pokémon data: {ex.Message}");
            return "Error fetching Pokémon data";
        }
    }
}
