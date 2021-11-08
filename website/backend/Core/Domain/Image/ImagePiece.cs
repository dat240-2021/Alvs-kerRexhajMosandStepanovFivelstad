using SharedKernel;

namespace backend.Core.Domain.Image
{
    public class ImagePiece : BaseEntity
    {
        public ImagePiece(byte[] imagedata, int sequenceNumber)
        {
            ImageData = imagedata;
            SequenceNumber = sequenceNumber;
        }

        public ImagePiece()
        {
            
        }
        
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int SequenceNumber { get; set; }
    }
}