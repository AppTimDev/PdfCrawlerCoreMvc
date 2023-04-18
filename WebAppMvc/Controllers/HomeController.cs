using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppMvc.Models;
using WebAppMvc.Extensions;
using WebAppMvc.ViewModels;
using System.IO;

namespace WebAppMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pdf()
        {
            PdfViewModel vm = new PdfViewModel();
            string webRootPath = _webHostEnvironment.WebRootPath;
            string dir = Path.Combine(webRootPath, "Download");
            //_logger.Info(dir);
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                foreach (FileInfo f in dirInfo.GetFiles("*.pdf", SearchOption.TopDirectoryOnly))
                {
                    _logger.Info(f.FullName);
                    _logger.Info(f.Name);
                    PdfFile pdf = new PdfFile();
                    pdf.FileName = f.Name;
                    pdf.DownloadPath = $"/Download/{f.Name}";
                    vm.PdfFiles.Add(pdf);
                }
            }
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}