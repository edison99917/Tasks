using System.ComponentModel.DataAnnotations.Schema;

namespace Edison.Models
{
    public class AssetCategory
    {
        public int ID { get; set; }

        public string? Name { get; set; }
    }
}
