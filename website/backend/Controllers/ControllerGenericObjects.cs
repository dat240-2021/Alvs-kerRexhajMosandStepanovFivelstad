
using System.ComponentModel.DataAnnotations;

namespace Controllers.Generics
{
    public record GenericResponseObject<T> {

        [Required]
        public T Data { get; set; }
        public string[] Errors {get; set;}
    }
}