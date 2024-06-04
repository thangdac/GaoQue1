using GaoQue.DataAccess;
using GaoQue.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GaoQue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public GPTDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGPTData()
        {
            var gptData = await _context.GPTDatas.ToListAsync(); // Truy vấn tất cả dữ liệu từ bảng GPTData
            return Ok(gptData);
        }
    }
}
