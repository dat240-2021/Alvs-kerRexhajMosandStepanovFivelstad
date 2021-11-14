using SharedKernel;

namespace backend.Core.Domain.Images
{

    public class ImageLabel : BaseEntity
    {
        public int Id {get; protected set;}

        public string Label { get; protected set; }
        public ImageCategory Category { get; protected set; }
        // public string Category { get=> _cat.Name; protected set {} }

        public ImageLabel(){}

        public ImageLabel(int id, string label,ImageCategory category)
        {
            Id = id;
            Label = label;
            Category = category;
        }
        public ImageLabel(string label,ImageCategory category)
        {
            Label = label;
            Category = category;
        }
    }
}