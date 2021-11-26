using System;
using System.Collections.Generic;
using SharedKernel;

namespace backend.Core.Domain.Images
{
    public class Image : BaseEntity
    {
        public int Id { get; protected set; }
        public string ImportId { get; set; }
        public Guid? UserId { get; set; }



        public ImageLabel Label { get; protected set; }
        // public string Label { get=> _lbl.Label; protected set {} }

        public string Category { get => Label.Category.Name; }


        public List<ImageSlice> Slices { get; protected set; } = new List<ImageSlice>();




        public Image() { }

        public Image(Guid user, ImageLabel label)
        {
            UserId = user;
            Label = label;
        }

        //Needed for adding parsed data from folders
        public Image(string importId)
        {
            ImportId = importId;
            Slices = new List<ImageSlice>();
        }



        public void AddImageSlice(byte[] data, int number)
        {
            Slices.Add(new ImageSlice(data, number));
        }
        public void SetLabel(ImageLabel item)
        {
            Label = item;
        }
    }
}