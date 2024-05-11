namespace Service_A.Models
{
    public class Configuration
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
