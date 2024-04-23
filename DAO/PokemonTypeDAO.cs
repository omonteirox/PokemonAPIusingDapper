using PokemonAPIusingDapper.Models;

namespace PokemonAPIusingDapper.DAO
{
    public interface IPokemonTypeDao
    {
        List<string> GetByNum(string num);
        string GetByTypeAndNum(string type, string num);
        bool Create(string type, string num);
        bool Delete(string num);
    }
}