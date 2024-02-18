namespace BackendTask.Models
{
    public class CategoryResponse
    {
        public string Name { get; set; }
        public Dictionary<string, CategoryResponse> Children { get; set; } // Navigation property for children
    }
}
