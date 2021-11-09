using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel;

namespace Domain.Image
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