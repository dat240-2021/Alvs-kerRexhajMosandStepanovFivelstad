using System.ComponentModel.DataAnnotations;
using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class ImageSlice : BaseEntity
    {
        [Key]
        public int Id {get; protected set;}
        public int SequenceNumber { get; protected set; }
        public byte[] ImageData { get; protected set; }

        public ImageSlice() {}

        public ImageSlice(byte[] imagedata, int sequenceNumber)
        {
            ImageData = imagedata;
            SequenceNumber = sequenceNumber;
        }

    }
}