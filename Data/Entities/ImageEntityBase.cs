using System.ComponentModel.DataAnnotations.Schema;

namespace BilliardManagement.Data.Entities
{
    public class ImageEntityBase
    {
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }
    }
}