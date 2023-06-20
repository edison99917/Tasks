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

        public int AssetCategoryID { get; set; }
        public AssetCategory? AssetCategory { get; set; }

        public int DepartmentID { get; set; }
        public Department? Department { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? SupplierName { get; set; }
    }
}
