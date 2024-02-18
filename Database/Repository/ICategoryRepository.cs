using BackendTask.Models;

namespace BackendTask.Database.Repository
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        public IEnumerable<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        Category? GetCategoryByName(string name);
        void AddChildCategory(Category parent, Category child);
        void DeleteCategory(Category? category);
    }
}
