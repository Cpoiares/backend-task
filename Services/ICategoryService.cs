using BackendTask.Models;

namespace BackendTask.Services
{
    public interface ICategoryService
    {
        public Dictionary<string, object> GetAllCategories();
        public void AddCategory(Category category);
        public void AddChildCategory(Category child, string? parentName);
        public void DeleteCategoryByName(string categoryName);
        public Category GetCategoryById(int id);
        public Category GetCategoryByName(string name);

    }
}
