using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.ViewModels
{
    public class CrawlerViewModel
    {
        public CrawlerViewModel()
        {
            WebImages = new List<WebImage>();
            WebPdfs  = new List<WebPdf>();
        }
        public List<WebImage> WebImages { get; set; }
        public List<WebPdf> WebPdfs { get; set; }

        [Display(Name = "URL")]
        [Required(ErrorMessage = "Url is required")]
        public string? Url { get; set; }
        public string? DownloadPath { get; set; }
        public int numNodesFind { get; set; } = 0;
        public int numImagesFind { get; set; } = 0;
        public int numImagesSave { get; set; } = 0;
        public int numPdfsFind { get; set; } = 0;
    }
    public class WebImage
    {
        public string src { get; set; }
        public string alt { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
    public class WebPdf
    {
        public string innertext { get; set; }
        public string filename { get; set; }
        public string outputfilepath { get; set; }
        public string downloadPath { get; set; }

    }
}
