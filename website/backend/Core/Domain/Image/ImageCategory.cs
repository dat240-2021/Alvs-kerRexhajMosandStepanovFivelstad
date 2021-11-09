using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Domain.Image
{

    public class ImageCategory : BaseEntity
    {
        public int Id {get; protected set;}

        public string Category { get; protected set; }

        public ImageCategory(string category) => Category=category;

        public ImageCategory(int id, string category)
        {
            Id = id;
            Category = category;
        }

    }
}