using GaoQue.DataAccess;
using GaoQue.Models;

namespace GaoQue.ViewModels
{
    public class OrderViewModel
    {
        public ApplicationUser User { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
