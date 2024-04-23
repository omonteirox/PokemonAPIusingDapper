using PokemonAPIusingDapper.Models;

namespace PokemonAPIusingDapper.DAO
{
    public interface IPokemonDao
    {
        List<Pokemon> GetAll();
        Pokemon GetByNum(string num);
        Pokemon Insert(Pokemon pokemon);
        Pokemon Update(Pokemon pokemon);
        bool Delete(string id);
    }
}