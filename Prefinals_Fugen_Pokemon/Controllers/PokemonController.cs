using Microsoft.AspNetCore.Mvc;
using Prefinals_Pokemon_Fugen.Models;
using Newtonsoft.Json.Linq;

namespace Prefinals_Pokemon_Fugen.Controllers
{

    public class PokemonController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokemonController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        public async Task<IActionResult> PokemonList(int page = 1)
        {
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon?offset={(page - 1) * 20}&limit=20");
            var pokemonList = JObject.Parse(response)["results"].ToObject<List<JObject>>();

            List<Pokemon> pokemons = new List<Pokemon>();
            foreach (var item in pokemonList)
            {
                pokemons.Add(new Pokemon { Name = item["name"].ToString() });
            }

            ViewBag.Page = page;
            return View(pokemons);
        }

        public async Task<IActionResult> Details(string name)
        {
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
            var pokemonData = JObject.Parse(response);

            var abilities = pokemonData["abilities"].Select(a => a["ability"]["name"].ToString()).ToList();
            var moves = pokemonData["moves"].Select(m => m["move"]["name"].ToString()).ToList();

            Pokemon pokemon = new Pokemon
            {
                Name = pokemonData["name"].ToString(),
                Abilities = abilities,
                Moves = moves
            };

            return View(pokemon);
        }
    }
}