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
            int sliceSize = 75;
            int newImageSize = 525;

            var sliceList = new List<byte[]>();
            for (var x = 0; x < (newImageSize / sliceSize); x++)
            {
                for (var y = 0; y < (newImageSize / sliceSize); y++)
                {
                    var resizedImg = SixLabors.ImageSharp.Image.Load(image);
                    resizedImg.Mutate(i => i.Resize(newImageSize,newImageSize));
                    var tempSlice = CreateSlice.CreateOneSlice(resizedImg, x*sliceSize, y*sliceSize, sliceSize);
                    sliceList.Add(tempSlice);
                }
            }
            return sliceList;
        }

    }

}