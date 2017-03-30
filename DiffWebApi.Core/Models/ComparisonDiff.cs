namespace DiffWebApi.Core.Models
{
    public class ComparisonDiff
    {
        public ComparisonDiff()
        {
            
        }

        public ComparisonDiff(long offset)
        {
            Offset = offset;
        }

        public ComparisonDiff(long offset, long length)
        {
            Offset = offset;
            Length = length;
        }

        public long Offset { get; set; }
        public long Length { get; set; }
    }
}
