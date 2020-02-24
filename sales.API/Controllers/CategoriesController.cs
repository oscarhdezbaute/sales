namespace sales.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Models;
    using Domain.Models;   

    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        public IQueryable<Category> GetCategories()
        {
            var list = this.db.Categories.OrderBy(c => c.Description);
            Category category;
            List<Category> categories = new List<Category>();
            foreach (var value in list)
            {
                category = new Category();
                category.CategoryId = value.CategoryId;
                category.Description = value.Description;
                category.ProductsCount = this.CountsProducts(value.CategoryId);
                category.ImagePath = value.ImagePath;
                categories.Add(category);
            }
            return categories.AsQueryable();
        }

        public int CountsProducts(int id)
        {
           return db.Products.Where(p => p.CategoryId == id).Count();           
        }
    }
}
