using System;
using System.Collections.Generic;
using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class Image : BaseEntity
    {
        public Image(string imageName, ImageCategory category, Guid? userId)
        {
            ImageName = imageName;
            Category = category;
            UserId = userId;
        }

        public Image()
        {
            
        }
        
        public int Id { get; protected set; }
        public string ImageName { get; set; }
        public Guid? UserId { get; set; }
        public ImageCategory Category { get; protected set; }
        private List<ImagePiece> imageList = new();
        public IEnumerable<ImagePiece> ImageList => imageList.AsReadOnly();

        public void AddImagePieces(ImagePiece item)
        {
            ImagePiece tempImagePiece = new(item.ImageData, item.SequenceNumber);
            imageList.Add(tempImagePiece);
        }
        
    }
    
}