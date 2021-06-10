using System.Threading.Tasks;
using Digikala.Core.Interfaces.Identity;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Digikala.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public HomeController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            return View(await _roleRepository.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string title)
        {
            return NotFound();
        }
    }
}
