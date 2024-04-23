namespace PokemonAPIusingDapper.Models
{
    public class ResponseModel<T>
    {
        public List<String> Errors { get; set; } = new List<string>();
        public T Data { get; set; }

        public ResponseModel(List<string> errors, T data)
        {
            Errors = errors;
            Data = data;
        }
        public ResponseModel(T data)
        {
            Data = data;
        }
        public ResponseModel(List<string> errors)
        {
            Errors = errors;
        }
        public ResponseModel(String error)
        {
            Errors.Add(error);
        }
    }
}