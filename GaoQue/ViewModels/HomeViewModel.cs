using GaoQue.Models;

namespace GaoQue.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; } 
        public List<Category> Categories { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Banner> Banners { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
