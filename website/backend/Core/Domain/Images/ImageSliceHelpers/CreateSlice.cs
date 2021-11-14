using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace backend.Core.Domain.Images.ImageSliceHelpers
{
    public class CreateSlice
    {
        //Takes in an image as Rgba32 and start xPicel and yPixel starting point for the slice to create, plus the sliceSize.
        //Each pixel that is not part of the slice to keep is set to Rgba = 0. The returned image is of type png (returned as byte[]).
        //The returned image is of total size 525x525 pixels, which is the same size as the original imaged passed in. SixLabors.ImageSharp library is used for the image manipulation. 
        public static byte[] CreateOneSlice(Image<Rgba32> image, int xPixel, int yPixel,int sliceSize)
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    if ((x >= xPixel && x < (xPixel + sliceSize)) && (y >= yPixel && y < (yPixel+sliceSize)))
                    {
                        
                    }
                    else
                    {
                        image[x, y] = Rgba32.ParseHex("#00000000");
                    }
                        
                }
            }
            var ms = new MemoryStream();
            image.Save(ms, PngFormat.Instance);
            return ms.ToArray();
        }
    }
}