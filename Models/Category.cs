using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendTask.Models
{
    public class Category
    {
        public Category()
        {
            Children = new List<Category>();
        }
        public int? CategoryId { get; set; } // Primary key
        public string Name { get; set; }
        [JsonIgnore]
        public int? ParentCategoryId { get; set; } // Foreign key to self
        [JsonIgnore]
        public Category? ParentCategory { get; set; } // Navigation property to parent category
        public List<Category> Children { get; set; } // Navigation property for children

    }
}
