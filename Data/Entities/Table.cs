namespace BilliardManagement.Data.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public ICollection<PriceList>? PriceList { get; set; }
    }
}