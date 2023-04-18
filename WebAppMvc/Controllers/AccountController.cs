using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppMvc.Data;
using WebAppMvc.ViewModels;

namespace WebAppMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiDbContext _context;

        public AccountController(ILogger<HomeController> logger, ApiDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public IActionResult Login()
        {
            var vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View("Login", loginViewModel);

            return RedirectToAction("Index", "Home");
        }
    }
}
