using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using SOProject.Repository;
using StackOverflowProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOProject.ServiceLayer
{
    public interface ICategoryService
    {

        void InsertCategory(CategoryViewModel cvm);
        void UpdateCategory(CategoryViewModel cvm);
        void DeleteCategory(int cid);
        List<CategoryViewModel> GetCategories();
        CategoryViewModel GetCategoriesByCategoryID(int cid);

    }

    public class CategoryService : ICategoryService
    {
        ICategoryRepository cr;
        public CategoryService(DbContextOptions<SOProjectDbContext> options)
        {
            cr = new CategoryRepository(options);
        }

        public void DeleteCategory(int cid)
        {
            cr.DeleteCategory(cid);
        }

        public List<CategoryViewModel> GetCategories()
        {
            List<Category> c = cr.GetCategories();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<CategoryViewModel> cvm = mapper.Map<List<Category>, List<CategoryViewModel>>(c);
            return cvm;
        }

        public CategoryViewModel GetCategoriesByCategoryID(int cid)
        {
            Category c = cr.GetCategoriesByCategoryID(cid);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            CategoryViewModel cvm = mapper.Map<Category, CategoryViewModel>(c);
            return cvm;
        }

        public void InsertCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category c = mapper.Map<CategoryViewModel, Category>(cvm);
            cr.InsertCategory(c);
        }

        public void UpdateCategory(CategoryViewModel cvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryViewModel, Category>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Category c = mapper.Map<CategoryViewModel, Category>(cvm);
            cr.UpdateCategory(c);
        }
    }
}
