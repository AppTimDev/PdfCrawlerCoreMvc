using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Xml;
using WebAppMvc.ViewModels;
using HtmlAgilityPack;
using WebAppMvc.Helpers;

namespace WebAppMvc.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CrawlerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult PdfCrawler()
        {
            CrawlerViewModel vm = new CrawlerViewModel();
            return View(vm);
        }

        
        [HttpPost]
        public async Task<IActionResult> PdfCrawler(CrawlerViewModel vm)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    string dir = null;
                    string url = vm.Url;
                    if (!string.IsNullOrEmpty(url))
                    {
                        Uri link = new Uri(url);
                        string html = Crawler.GetHtml(link);
                        if (!string.IsNullOrEmpty(html))
                        {
                            HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
                            _doc.LoadHtml(html);
                            HtmlNodeCollection nodes = _doc.DocumentNode.SelectNodes("//a");
                            if (nodes != null)
                            {
                                //string webRootPath = _webHostEnvironment.WebRootPath;
                                string contentRootPath = _webHostEnvironment.ContentRootPath;
                                dir = Path.Combine(contentRootPath, "Upload");
                                string href;
                                string filename;
                                string filepath;
                                string pdflink;
                                string prefix;
                                string innertext;
                                int count = 0;
                                foreach (HtmlNode node in nodes)
                                {

                                    href = node.GetAttributeValue("href", "");
                                    if (href.IndexOf(".pdf") >= 0)
                                    {
                                        count++;
                                        prefix = "D" + count.ToString() + "-";
                                        innertext = node.InnerText;
                                        filename = Path.GetFileName(href);  //cannot contain some symbol e.g. /\
                                        filepath = Path.Combine(dir, prefix + filename + ".pdf");
                                        if (href.IndexOf("http") < 0)
                                        {
                                            if (href.StartsWith("//"))
                                            {
                                                pdflink = "https://" + href.TrimStart('/');
                                            }
                                            else if (href.StartsWith("/"))
                                            {
                                                pdflink = "https://" + link.Host + href;
                                            }
                                            else if (!href.Contains("/"))
                                            {
                                                Uri pdfUri = new Uri(link, href);
                                                pdflink = pdfUri.AbsoluteUri;
                                            }
                                            else
                                            {
                                                pdflink = href;
                                            }
                                        }
                                        else
                                        {
                                            pdflink = href;
                                        }
                                        WebPdf WebPdf = new WebPdf();
                                        WebPdf.outputfilepath = filepath;
                                        WebPdf.downloadPath = pdflink;
                                        WebPdf.filename = filename;
                                        WebPdf.innertext = innertext;
                                        vm.WebPdfs.Add(WebPdf);
                                    }
                                }
                                try
                                {
                                    //task parallel download pdf
                                    Crawler.SaveWebPdfs(vm.WebPdfs);
                                }
                                catch (Exception e)
                                {
                                    //logger.Info("-- ScraperController : ScrapePdfs exception--");
                                    //logger.Error(e);
                                }
                            }
                        }
                    }
                    vm.numPdfsFind = vm.WebPdfs.Count;
                    if(vm.numPdfsFind > 0)
                    {
                        vm.DownloadPath = dir;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ModelState.AddModelError("", "Failed!");
            }
            //TempData["Error"] = "testing error";
            return View("PdfCrawler", vm);
        }
    }
}
