using SharedKernel;

namespace backend.Core.Domain.Images
{

    public class ImageCategory : BaseEntity
    {
        public int Id {get; protected set;}

        public string Name { get; protected set; }

        public ImageCategory() {}
        public ImageCategory(string category) => Name=category;

        public ImageCategory(int id, string category)
        {
            Id = id;
            Name = category;
        }

    }
}