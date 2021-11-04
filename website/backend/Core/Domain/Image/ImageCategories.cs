using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class ImageCategories : BaseEntity
    {
        public ImageCategories(string category)
        {
            Category = category;
        }

        public ImageCategories()
        {
            
        }
        
        public int Id { get; protected set; }
        public string Category { get; protected set; }
    }
}