using GaoQue.DataAccess;
using GaoQue.Models;
using GaoQue.ViewModels;

namespace GaoQue.ViewModels
{
    public class ContactViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<ApplicationUser> Logins  { get; set;}
    }
}
