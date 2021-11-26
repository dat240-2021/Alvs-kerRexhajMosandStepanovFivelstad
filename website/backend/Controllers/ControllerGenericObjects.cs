
using System.ComponentModel.DataAnnotations;

namespace Controllers.Generics
{
    public record GenericResponseObject<T>
    {

        public GenericResponseObject(T data)
        {
            Data = data;
        }

        public GenericResponseObject(string[] errors)
        {
            Errors = errors;
        }

        [Required]
        public T Data { get; set; }
        public string[] Errors { get; set; }
    }
}