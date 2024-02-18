using BackendTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackendTask.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SampleDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(c => c.Children)
                    .WithOne(c => c.ParentCategory)
                    .HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(e => e.CategoryId);
       
                entity.Property(e => e.CategoryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CategoryId");
            });

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Postcards" },
                new Category { CategoryId = 2, Name = "5 inch x 6 inch Postcards", ParentCategoryId = 1 },
                new Category { CategoryId = 3, Name = "6 inch x 7 inch Postcards", ParentCategoryId = 1 }
            );
        }
    }
}
