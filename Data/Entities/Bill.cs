namespace BilliardManagement.Data.Entities
{
    public class Bill
    {
        public int BillId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime DateTimeOfCreated { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public string? PaymentMethod { get; set; }
        public int Status { get; set; }
    }
}