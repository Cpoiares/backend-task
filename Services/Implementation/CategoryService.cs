using BackendTask.Database.Repository;
using BackendTask.Models;
using IdentityServer4.Extensions;

namespace BackendTask.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public Dictionary<string, object> GetAllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();

            return MapCategories(categories);

        }
        private static IDictionary<string, object> FlattenCategoryHierarchy(Category category)
        {
            var categoryData = new Dictionary<string, object>();
            if(category.Children != null)
            { 
                foreach (var childCategory in category.Children)
                {
                    categoryData[childCategory.Name] = FlattenCategoryHierarchy(childCategory);
                }
            }
            return categoryData;
        }

        private static Dictionary<string, object> MapCategories(IEnumerable<Category> categories)
        {
            var transformedCategories = new Dictionary<string, object>();
            foreach (var category in categories)
            {
                transformedCategories[category.Name] = FlattenCategoryHierarchy(category);
            }

            return transformedCategories;
        }

        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }

        public void DeleteCategoryByName(string categoryName)
        {
            Category category = _categoryRepository.GetCategoryByName(categoryName);
            if(category != null)
                _categoryRepository.DeleteCategory(category);
            else
                throw new Exception("Category not found");
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public Category GetCategoryByName(string name)
        {
            return _categoryRepository.GetCategoryByName(name);
        }

        public void AddChildCategory(Category child, string parentName)
        {
            if (parentName.IsNullOrEmpty())
            {
                _categoryRepository.AddCategory(child);
            }
            else
            {
                Category parent = _categoryRepository.GetCategoryByName(parentName);
                if (parent != null)
                {
                    child.ParentCategoryId = parent.CategoryId;
                    _categoryRepository.AddChildCategory(child, parent);
                }
                else
                {
                    throw new Exception("Parent category not found");
                }
            }
        }
    }
}
