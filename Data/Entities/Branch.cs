namespace BilliardManagement.Data.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int TableNumber { get; set; }

        public Club? Club { get; set; }
        public int ClubId { get; set; }

        public ICollection<PriceList>? PriceList { get; set; }
    }
}