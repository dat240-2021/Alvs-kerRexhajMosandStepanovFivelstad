using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel;

namespace Domain.Image
{
    public class Image : BaseEntity
    {
        public int Id { get; protected set; }
        public string ImageName { get; set; }
        public Guid? UserId { get; set; }
        public ImageCategory Category { get; protected set; }
        public List<ImageSlice> Slices { get; protected set; }

        public Image(){}

        public Image(string imageName, ImageCategory category, Guid? userId)
        {
            ImageName = imageName;
            Category = category;
            UserId = userId;
        }

        //Needed for adding parsed data from folders
        public Image(string imageName)
        {
            ImageName = imageName;
            Slices = new List<ImageSlice>();
        }



        public void AddImageSlice(byte[] data, int number)
        {
            Slices.Add(new ImageSlice(data,number));
        }
        public void SetCategory(ImageCategory item)
        {
            Category = item;
        }
    }
}