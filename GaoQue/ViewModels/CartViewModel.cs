using GaoQue.DataAccess;
using GaoQue.Models; // Thêm namespace này

namespace GaoQue.ViewModels
{
    public class CartViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<ApplicationUser> Logins { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<Order> Orders { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public Order RecentOrder { get; set; }
    }
}
