using Dapper;
using Npgsql;
using PokemonAPIusingDapper.DAO;
using PokemonAPIusingDapper.Exceptions;
using System.Data;

namespace PokemonAPIusingDapper.Services
{
    public class PokemonTypeService : IPokemonTypeDao
    {
        private readonly IDbConnection _connection;
        public PokemonTypeService(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public List<string> GetByNum(string num)
        {
            try
            {
                return _connection.Query<string>("SELECT name FROM pokemon_type WHERE pokemon_num = @Num", new { Num = num }).AsList();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e.InnerException);
            }
        }

        public string GetByTypeAndNum(string type, string num)
        {
            try
            {
                return _connection.QueryFirstOrDefault<string>("SELECT name FROM pokemon_type WHERE name = @Type AND pokemon_num = @Num", new { Type = type, Num = num });
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e.InnerException);
            }
        }

        public bool Create(string type, string num)
        {
            try
            {
                int affectedRows = _connection.Execute("INSERT INTO pokemon_type (name, pokemon_num) VALUES (@Type, @Num)", new { Type = type, Num = num });
                return affectedRows > 0;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e.InnerException);
            }
        }

        public bool Delete(string num)
        {
            try
            {
                int affectedRows = _connection.Execute("DELETE FROM pokemon_type WHERE pokemon_num = @Num", new { Num = num });
                return affectedRows > 0;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e.InnerException);
            }
        }
    }
}