namespace DiffWebApi.Core.Models
{
    public class PositionPair
    {
        public PositionPair(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public byte[] LeftPart { get; set; }
        public byte[] RightPart { get; set; }
    }
}
