namespace WebAppMvc.ViewModels
{
    public class PdfViewModel
    {
        public List<PdfFile> PdfFiles { get; set; }
        public PdfViewModel()
        {
            PdfFiles = new List<PdfFile>();
        }
    }
    public class PdfFile
    {
        public string? FileName { get; set; }
        public string? DownloadPath { get; set; }
    }
}
