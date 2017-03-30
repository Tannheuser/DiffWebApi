namespace DiffWebApi.Web.Data
{
    public class Base64Dto
    {
        public Base64Dto(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
    }
}