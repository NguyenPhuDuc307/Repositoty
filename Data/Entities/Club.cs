namespace BilliardManagement.Data.Entities
{
    public class Club : ImageEntityBase
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Branch>? Branches { get; set; }
    }
}