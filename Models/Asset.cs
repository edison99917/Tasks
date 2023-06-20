using System.ComponentModel.DataAnnotations;

namespace Edison.Models
{
    public class Asset
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? AssetCode { get; set; }

        [Required]
        public string? AssetName { get; set; }

        public AssetCategory? AssetCategory { get; set; }

        public Department? Department { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal Price { get; set; }

        public string? SupplierName { get; set; }
    }
}
