using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class ImagePieces : BaseEntity
    {
        public ImagePieces(byte[] imagedata, int sequenceNumber)
        {
            ImageData = imagedata;
            SequenceNumber = sequenceNumber;
        }

        public ImagePieces()
        {
            
        }
        
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int SequenceNumber { get; set; }
    }
}