using System.Collections.Generic;
using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class Image : BaseEntity
    {
        public Image(string imageName, ImageCategories categories)
        {
            ImageName = imageName;
            Categories = categories;
        }

        public Image()
        {
            
        }
        
        public int Id { get; protected set; }
        public string ImageName { get; protected set; }
        public ImageCategories Categories { get; protected set; }
        private List<ImagePieces> imageList = new();
        public IEnumerable<ImagePieces> ImageList => imageList.AsReadOnly();

        public void AddImagePieces(ImagePieces item)
        {
            ImagePieces tempImagePiece = new(item.ImageData, item.SequenceNumber);
            imageList.Add(tempImagePiece);
        }
        
    }
    
}