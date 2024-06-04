using GaoQue.Models;
using GaoQue.DataAccess;
using X.PagedList;
using GaoQue.ViewModels;

namespace GaoQue.ViewModels
{
    public class ProductViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<ApplicationUser> Logins { get; set; }
        public List<ProductInformation> Information { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ProductInformation> Informations { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IPagedList<Product> PagedProducts { get; set; }
        public Product Product { get; set; }
    }
}
