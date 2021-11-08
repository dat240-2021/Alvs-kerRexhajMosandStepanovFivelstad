using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class ImageCategory : BaseEntity
    {
        public ImageCategory(string category)
        {
            Category = category;
        }

        public ImageCategory()
        {
            
        }
        
        public int Id { get; protected set; }
        public string Category { get; set; }
    }
}