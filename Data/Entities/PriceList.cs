using System.ComponentModel.DataAnnotations.Schema;

namespace BilliardManagement.Data.Entities
{
    public class PriceList
    {
        public Table? Table { get; set; }
        public int TableId { get; set; }

        public Branch? Branch { get; set; }
        public int BranchId { get; set; }

        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
    }
}