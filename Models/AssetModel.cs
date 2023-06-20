namespace Edison.Models
{
    public class AssetModel
    {
        public Asset? Asset { get; set; }

        public List<AssetCategory>? AssetCategories { get; set; }

        public List<Department>? Departments { get; set; }
    }
}
