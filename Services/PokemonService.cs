using Dapper;
using PokemonAPIusingDapper.DAO;
using PokemonAPIusingDapper.Models;
using System.Data;
using Newtonsoft.Json;
using PokemonAPIusingDapper.Exceptions;
using Npgsql;

namespace PokemonAPIusingDapper.Services
{
    public class PokemonService : IPokemonDao
    {
        private readonly IDbConnection _connection;
        public PokemonService(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Pokemon> GetAll()
        {
            try
            {
                return _connection.Query<Pokemon>(@"SELECT * FROM pokemon ORDER BY num").ToList();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message);
            }
        }

        public Pokemon GetByNum(string num)
        {
            try
            {
                var pokemon = _connection.QueryFirstOrDefault<Pokemon>(@"SELECT * FROM pokemon WHERE num = @Num", new { Num = num });
                if (pokemon != null)
                {

                    pokemon.PrevEvolution= GetEvolutions(pokemon.PrevEvolution.ToString());
                    pokemon.PrevEvolution = GetEvolutions(pokemon.NextEvolution.ToString());
                }
                return pokemon;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message);
            }
        }

      

        public Pokemon Insert(Pokemon pokemon)
        {
            string query = @"
        INSERT INTO pokemon (num, name, pre_evolution, next_evolution) 
        VALUES (@Num, @Name, @PrevEvolutionJson::json, @NextEvolutionJson::json) 
        RETURNING id;"; 

            try
            {
                var id = _connection.QuerySingle<int>(query, new
                {
                    pokemon.Num,
                    pokemon.Name,
                    PrevEvolutionJson = ConvertToJson(pokemon.PrevEvolution),
                    NextEvolutionJson = ConvertToJson(pokemon.NextEvolution)
                });
                pokemon.Id = id;
                return pokemon;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message);
            }
        }

        public Pokemon Update(Pokemon pokemon)
        {
            string query = @"
        UPDATE pokemon 
        SET name = @Name, pre_evolution = @PrevEvolutionJson::json, next_evolution = @NextEvolutionJson::json 
        WHERE num = @Num;";

            try
            {
                _connection.Execute(query, new
                {
                    pokemon.Name,
                    PrevEvolutionJson = ConvertToJson(pokemon.PrevEvolution),
                    NextEvolutionJson = ConvertToJson(pokemon.NextEvolution),
                    pokemon.Num
                });
                return pokemon;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message);
            }
        }
        public List<Pokemon> GetByPage(int page, int qtd)
        {
            string query = @"SELECT * FROM POKEMON LIMIT ? OFFSET ?";
            try
            {
                return _connection.Query<Pokemon>(query).ToList();
            }
            catch(Exception e)
            {
                throw new DatabaseException(e.Message);
            }
            
        }
        private string ConvertToJson(List<Evolution> evolutions)
        {
            
            return JsonConvert.SerializeObject(evolutions);
        }
        private List<Evolution> GetEvolutions(string json)
        {
            if (string.IsNullOrEmpty(json) || json == "[{}]")
                return new List<Evolution>();
            
            List<Evolution> x = JsonConvert.DeserializeObject<List<Evolution>>(json);
            if (x == null)
                return new List<Evolution>();
            return x;
            
        }

        public List<Pokemon> GetByType(string type)
        {
            string query = @"
        SELECT pokemon.* 
        FROM pokemon 
        INNER JOIN pokemon_type 
            ON pokemon_type.pokemon_num = pokemon.num 
        WHERE pokemon_type.name = @Type 
        ORDER BY pokemon.num";
            try
            {
                return _connection.Query<Pokemon>(query, new { Type = type }).ToList();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message);
            }

        }
        
    }
}