using Microsoft.AspNetCore.Mvc;
using PokemonAPIusingDapper.Models;
using PokemonAPIusingDapper.Services;

namespace PokemonAPIusingDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _pokemonService;
        private readonly PokemonTypeService _pokemonTypeService;
        public PokemonController(PokemonService pokemonService, PokemonTypeService pokemonTypeService)
        {
            _pokemonService = pokemonService;
            _pokemonTypeService = pokemonTypeService;
        }
        [HttpGet("{num}")]
        public ActionResult<ResponseModel<Pokemon>> GetByNum(string num)
        {
            try
            {
                var pokemon = _pokemonService.GetByNum(num);
                if (pokemon == null)
                {
                    return NotFound();
                }
                pokemon.Type = _pokemonTypeService.GetByNum(pokemon.Num);
                return Ok(new ResponseModel<Pokemon>(pokemon));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }
        [HttpPost]
        public ActionResult<ResponseModel<Pokemon>> Create([FromBody] Pokemon pokemon)
        {
            try
            {
                var newPokemon = _pokemonService.Insert(pokemon);
                if (newPokemon == null)
                {
                    return BadRequest();
                }
                foreach (var type in pokemon.Type)
                {
                    _pokemonTypeService.Create(type, pokemon.Num);
                }
                return Ok(new ResponseModel<Pokemon>(newPokemon));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var deleted = _pokemonService.Delete(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }

        [HttpPut]
        public ActionResult<ResponseModel<Pokemon>> Update([FromBody] Pokemon pokemon)
        {
            try
            {
                var updatedPokemon = _pokemonService.Update(pokemon);
                if (updatedPokemon == null)
                {
                    return NotFound();
                }
                _pokemonTypeService.Delete(pokemon.Num);
                foreach (var type in pokemon.Type)
                {
                    _pokemonTypeService.Create(type, pokemon.Num);
                }
                return Ok(new ResponseModel<Pokemon>(updatedPokemon));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }

        [HttpGet]
        public ActionResult<List<Pokemon>> GetAll()
        {
            try
            {
                var pokemons = _pokemonService.GetAll();
                if (pokemons == null)
                {
                    return Ok(new List<Pokemon>());
                }
                foreach (var pokemon in pokemons)
                {
                    pokemon.Type = _pokemonTypeService.GetByNum(pokemon.Num);
                }
                return Ok(new ResponseModel<List<Pokemon>>(pokemons));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }

        [HttpGet("type/{type}")]
        public ActionResult<List<Pokemon>> GetByType(string type)
        {
            try
            {
                var pokemons = _pokemonService.GetByType(type);
                if (pokemons == null)
                {
                    return NotFound();
                }
                foreach (var pokemon in pokemons)
                {
                    pokemon.Type = _pokemonTypeService.GetByNum(pokemon.Num);
                }
                return Ok(new ResponseModel<List<Pokemon>>(pokemons));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }

        [HttpGet("{page}/{qtd}")]
        public ActionResult<List<Pokemon>> GetByPage(int page, int qtd)
        {
            try
            {
                var pokemons = _pokemonService.GetByPage(page, qtd);
                if (pokemons == null)
                {
                    return NotFound();
                }
                foreach (var pokemon in pokemons)
                {
                    pokemon.Type = _pokemonTypeService.GetByNum(pokemon.Num);
                }
                return Ok(new ResponseModel<List<Pokemon>>(pokemons));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseModel<Exception>(e.Message));
            }
        }
    }
}