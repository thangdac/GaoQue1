using GaoQue.DataAccess;
using GaoQue.Models;

namespace GaoQue.ViewModels
{
    public class BannerViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<ApplicationUser> Logins { get; set; }
        public List<GPTData> GPTDatas { get; set; }
    }
}
