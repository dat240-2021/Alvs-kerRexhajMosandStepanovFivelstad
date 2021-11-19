using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace backend.Core.Domain.Images.ImageSliceHelpers
{
    public class TempSlice{
        public Image<Rgba32> image;

        public int pixels_written;
        public bool too_small {get{
            float n_pixels = image.Width * image.Height;
            return (float)pixels_written/n_pixels < 0.005;
        } set{}}

        public TempSlice(Image<Rgba32> img){
            image = img;
            pixels_written = 0;
        }
    }

    public class ManualSlicer
    {
        //Takes in an image as byte[] and resizes it to 525x525 pixels. Passes in this resized image to the CreateSlice method which creates one slice, which is added to the sliceList.
        //This process is repeated to create 49 slices of 75x75 pixels. SixLabors.ImageSharp library is used for the image manipulation.
        public List<byte[]> ManualSlice(byte[] image,byte[] sliceFile,string[] colors)
        {
            var img = SixLabors.ImageSharp.Image.Load(image);
            var imgSlc = SixLabors.ImageSharp.Image.Load(sliceFile);
            imgSlc.Mutate(x => x.Resize(img.Width, img.Height));

            var slicesDict = new Dictionary<Rgba32, TempSlice>();

            foreach(var color in colors){
                slicesDict.TryAdd(Rgba32.ParseHex(color.Remove(0,1).ToUpper()+"FF"), new TempSlice(new Image<Rgba32>(img.Width, img.Height)));
            }

            for (var x = 0; x < img.Width; x++)
            {
                for (var y = 0; y < img.Height; y++)
                {
                    byte range = 10;
                    var p = imgSlc[x,y];
                    var updateSlice = slicesDict.Where(x=>
                    x.Key.R<p.R+range && x.Key.R>p.R-range &&
                    x.Key.G<p.G+range && x.Key.G>p.G-range &&
                    x.Key.B<p.B+range && x.Key.B>p.B-range
                    ).FirstOrDefault();;

                    if (updateSlice.Value!=null){
                        updateSlice.Value.image[x,y] = img[x,y];
                        updateSlice.Value.pixels_written++;
                    }
                }
            }


            var sliceList = new List<byte[]>();


            foreach(var slice in slicesDict.Where(s => s.Value.too_small==true )){
                var write_to = slicesDict.Where(s => s.Value.too_small==false ).FirstOrDefault();

                for (var x = 0; x < img.Width; x++)
                    {
                    for (var y = 0; y < img.Height; y++)
                        {
                            var pixel = slice.Value.image[x,y];
                            if (pixel.ToHex() !="00000000" )
                                write_to.Value.image[x,y] = pixel;
                        }
                    }

                slicesDict.Remove(slice.Key);
            }

            // Convert to png and save.
            foreach(var slice in slicesDict){

                var ms = new MemoryStream();
                slice.Value.image.Save(ms, PngFormat.Instance);
                sliceList.Add(ms.ToArray());
            }

            return sliceList;
        }






    }

}