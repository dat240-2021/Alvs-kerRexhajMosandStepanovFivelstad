using System.Collections.Generic;
using SixLabors.ImageSharp.Processing;

namespace backend.Core.Domain.Images.ImageSliceHelpers
{
    public class SliceImage
    {
        //Takes in an image as byte[] and resizes it to 525x525 pixels. Passes in this resized image to the CreateSlice method which creates one slice, which is added to the sliceList.
        //This process is repeated to create 49 slices of 75x75 pixels. SixLabors.ImageSharp library is used for the image manipulation.
        public List<byte[]> Slice(byte[] image)
        {
            var img = SixLabors.ImageSharp.Image.Load(image);
            var sliceSize = (img.Width/7,img.Height/7);
            var sliceList = new List<byte[]>();

            for (var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 7; y++)
                {
                    var tempSlice = CreateSlice.CreateOneSlice(img.Clone(), x*sliceSize.Item1, y*sliceSize.Item2, sliceSize);
                    sliceList.Add(tempSlice);
                }
            }
            return sliceList;
        }

    }

}