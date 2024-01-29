namespace WebApi.Domain.Entities
{
    public class JsonFile
    {
        public JsonFile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string JsonData { get; set; }
    }
}
