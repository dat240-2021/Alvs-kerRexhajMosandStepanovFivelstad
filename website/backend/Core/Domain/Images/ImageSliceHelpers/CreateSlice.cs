using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;


namespace backend.Core.Domain.Images.ImageSliceHelpers
{
    public class CreateSlice
    {
        //Takes in an image as Rgba32 and start xPicel and yPixel starting point for the slice to create, plus the sliceSize.
        //Creates a blank image of the same size as the input image and copies the needed pixels from the original image to the new image.
        //Return the resulting slice.w SixLabors.ImageSharp library is used for the image manipulation.
        public static byte[] CreateOneSlice(Image<Rgba32> image, int xPixel, int yPixel, (int, int) sliceSize)
        {
            var resultImage = new Image<Rgba32>(image.Width, image.Height);

            for (var x = xPixel; x < (xPixel + sliceSize.Item1); x++)
            {
                for (var y = yPixel; y < (yPixel + sliceSize.Item2); y++)
                {
                    resultImage[x, y] = image[x, y];
                }
            }

            var ms = new MemoryStream();
            resultImage.Save(ms, PngFormat.Instance);
            return ms.ToArray();
        }
    }
}