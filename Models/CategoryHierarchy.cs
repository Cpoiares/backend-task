namespace BackendTask.Models
{
    public class CategoryHierarchy
    {
        public int CategoryHierarchyId { get; set; } // Primary key
        public int ParentCategoryId { get; set; }
        public int ChildCategoryId { get; set; }
    }
}
