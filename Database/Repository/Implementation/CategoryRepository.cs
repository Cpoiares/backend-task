using BackendTask.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendTask.Database.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddCategory(Category category)
        {
            var aux = new Category
            {
                Name = category.Name,
            };

            _context.Categories.Add(aux);
            _context.SaveChanges();
        }

        public void AddChildCategory(Category child, Category parent)
        {
            var aux = new Category
            {
                Name = child.Name,
                ParentCategoryId = child.ParentCategoryId
            };
            _context.Categories.Add(aux);
            parent.Children.Add(aux);
            _context.Update(parent);
            _context.SaveChanges();
        }

        public void DeleteCategory(Category? category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            List<Category> categories = _context.Categories.ToList();

            List<Category> result = new List<Category>();

            foreach (var category in categories.Where(c => c.ParentCategoryId == null))
            {
                LoadCategoryAndChildren(category);
                result.Add(category);
            }

            return result;
        }

        private void LoadCategoryAndChildren(Category category)
        {
            _context.Entry(category)
                .Collection(c => c.Children)
                .Load();

            if (category.Children != null)
            {
                foreach (var child in category.Children)
                {
                    LoadCategoryAndChildren(child);
                }
            }
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public Category? GetCategoryByName(string name)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Name == name);
            LoadCategoryAndChildren(category);
            return category;
        }
    }
}
