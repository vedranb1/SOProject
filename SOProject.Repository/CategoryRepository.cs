using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOProject.Repository
{
    public interface ICategoryRepository
    {

        void InsertCategory(Category c);
        void UpdateCategory(Category c);
        void DeleteCategory(int cid);
        List<Category> GetCategories();
        Category GetCategoriesByCategoryID(int cid);

    }

    public class CategoryRepository : ICategoryRepository
    {
        SOProjectDbContext db;

        public CategoryRepository(DbContextOptions<SOProjectDbContext> options)
        {
            db = new SOProjectDbContext(options);
        }

        public void DeleteCategory(int cid)
        {
            Category cat = db.Categories.Where(temp => temp.CategoryID == cid).FirstOrDefault();
            if(cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
            }
        }

        public List<Category> GetCategories()
        {
            List<Category> cat = db.Categories.ToList();
            return cat;
        }

        public Category GetCategoriesByCategoryID(int cid)
        {
            Category cat = db.Categories.Where(temp => temp.CategoryID == cid).FirstOrDefault();
            return cat;
        }

        public void InsertCategory(Category c)
        {
            db.Categories.Add(c);
            db.SaveChanges();
        }

        public void UpdateCategory(Category c)
        {
            Category cat = db.Categories.Where(temp => temp.CategoryID == c.CategoryID).FirstOrDefault();
            if(cat != null)
            {
                cat.CategoryName = c.CategoryName;
                db.SaveChanges();
            }
        }
    }
}
