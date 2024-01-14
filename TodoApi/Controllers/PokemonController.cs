using Microsoft.AspNetCore.Mvc;
using TodoApi.APIs;
using System.Text.Json;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PokemonController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<PokemonController> _logger;

    public PokemonController(ILogger<PokemonController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("testing")]
    public String Testing()
    {
        return "does this count";
    }
    


    [HttpGet("pokemon/{nameORid}")]
    public async Task<ActionResult<string>> GetPokemonInfo(string nameORid)
    {
        try
        {
            // Fetch data from PokeAPI
            string pokemonData = await PokeAPIFetcher.GetPokemonInfo(nameORid);
            if (pokemonData == null)
            {
                return NotFound($"No Pokémon found with the name {nameORid}.");
            }

            // Extract the Pokémon's description (pseudo-code)
            string description = ExtractDescription(pokemonData);
           // string description = ExtractAllEnglishDescriptions(pokemonData);
            Console.WriteLine("pokeAPIdes:"+ description);

            // Translate description to Shakespearean
            string shakespeareanDescription = await ShakespeareTranslator.TranslateToShakespearean(description);
             Console.WriteLine("shakespeareanDescription:"+ shakespeareanDescription);
            if (shakespeareanDescription == null)
            {
                return Problem("There was a problem translating the description.");
            }

            // removing escape characters for now as it doesnt seem to work on my page
             var response = new { nameORid = nameORid, Description = shakespeareanDescription.Replace("\n", " ").Replace("\f", " ") };
             
             return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem($"An error occurred: {ex.Message}");
        }
    }


public static string ExtractDescription(string json)
{
    try
    {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement flavorTextEntries = root.GetProperty("flavor_text_entries");

            foreach (JsonElement entry in flavorTextEntries.EnumerateArray())
            {
                if (entry.GetProperty("language").GetProperty("name").GetString() == "en")
                {
                    String? description = entry.GetProperty("flavor_text").GetString();

                    if(description==null){
                        return "no description found";
                    }
                    
                    
                    if(description!=null){
                        return description.Replace("\n", " ").Replace("\f", " ");
                    }else{
                        return "No English description available.";
                    }
                    //dead code
                    return description;
                    
                }
            }

            return "No English description available.";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing JSON: {ex.Message}");
        return "Error parsing JSON";
    }
}



public static string ExtractAllEnglishDescriptions(string json)
{
    try
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            JsonElement root = doc.RootElement;
            JsonElement flavorTextEntries = root.GetProperty("flavor_text_entries");

            string allDescriptions = "";
            foreach (JsonElement entry in flavorTextEntries.EnumerateArray())
            {
                if (entry.GetProperty("language").GetProperty("name").GetString() == "en")
                {
                    string description = entry.GetProperty("flavor_text").GetString();
                    // Append this description to the accumulated descriptions
                    if(description!=null){
                        return description.Replace("\n", " ").Replace("\f", " ");
                    }else{
                        return "No English description available.";
                    }
                }
            }

            return string.IsNullOrWhiteSpace(allDescriptions) ? "No English descriptions available." : allDescriptions.Trim();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing JSON: {ex.Message}");
        return null;
    }
}




}
