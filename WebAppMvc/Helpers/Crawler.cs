using System.Net;
using WebAppMvc.ViewModels;
//using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMvc.Helpers
{
    public static class Crawler
    {
        private static string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
        //private static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string GetHtml(Uri url)
        {
            StreamReader streamReader = null;
            Stream dataStream = null;
            HttpWebResponse response = null;
            try
            {
                //request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "GET";
                request.UserAgent = _userAgent;
                //request.Headers.Add("key", "tset");
                //Cookie cookie = new Cookie();
                //cookie.Name = "";
                //if (request.CookieContainer == null)
                //{
                //    request.CookieContainer = new CookieContainer();
                //}

                //request.CookieContainer.Add(cookie);

                response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();
                streamReader = new StreamReader(dataStream);

                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
                if (dataStream != null) dataStream.Close();
                if (response != null) response.Close();
            }
        }
        public static Image DownloadImage(Uri url)
        {
            Image result = null;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.UserAgent = _userAgent;
                WebResponse webResponse = webRequest.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    using (Image tempImage = Image.FromStream(webStream))
                    {
                        result = new Bitmap(tempImage);
                    }
                }
                webResponse.Close();
            }
            catch (Exception e)
            {
                //_logger.Info($"DownloadImage fail, url:{url.ToString()}");
                //_logger.Error(e);
                return null;
            }
            return result;
        }

        public static string GetMimeType(this Image image)
        {
            return image.RawFormat.GetMimeType();
        }

        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            try
            {
                return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void SaveWebImage(string downloadPath, string outputfilepath)
        {
            try
            {
                using (Image image = DownloadImage(new Uri(downloadPath)))
                {
                    if (image != null)
                    {
                        string type = image.RawFormat.GetMimeType();
                        if (type == ImageFormat.Jpeg.GetMimeType())
                        {
                            image.Save(outputfilepath, ImageFormat.Jpeg);
                        }
                        else if (type == ImageFormat.Png.GetMimeType())
                        {
                            image.Save(outputfilepath, ImageFormat.Png);
                        }
                        else if (type == ImageFormat.Gif.GetMimeType())
                        {
                            image.Save(outputfilepath, ImageFormat.Gif);
                        }
                        else if (type == ImageFormat.Bmp.GetMimeType())
                        {
                            image.Save(outputfilepath, ImageFormat.Bmp);
                        }
                        else
                        {
                            image.Save(outputfilepath);
                        }
                    }
                    else
                    {
                        //_logger.Info($"SaveWebImage: no image is downloaded!!");
                        //_logger.Info($"downloadPath: {downloadPath}");
                        //_logger.Info($"outputfilepath: {outputfilepath}");
                        throw new Exception("SaveWebImage: no image is downloaded!!");
                    }
                }
            }
            catch (Exception e)
            {
                //_logger.Info($"SaveWebImage fail!!");
                //_logger.Info($"downloadPath: {downloadPath}");
                //_logger.Info($"outputfilepath: {outputfilepath}");
                //_logger.Error(e);
                //rethrow exception
                throw;
            }

        }

        public static void SaveWebPdf(string downloadPath, string outputfilepath)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    wc.Headers["User-Agent"] = _userAgent;
                    wc.DownloadFile(new Uri(downloadPath), outputfilepath);
                }
            }
            catch (Exception e)
            {
                //_logger.Info($"SaveWebPdf fail!!");
                //_logger.Info($"downloadPath: {downloadPath}");
                //_logger.Info($"outputfilepath: {outputfilepath}");
                //_logger.Error(e);
                //rethrow exception
                throw;
            }
        }

        public static void SaveWebPdfs(List<WebPdf> WebPdfs)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        Parallel.ForEach(WebPdfs, (pdf) =>
                        {
                            try
                            {
                                SaveWebPdf(pdf.downloadPath, pdf.outputfilepath);
                            }
                            catch (Exception e)
                            {
                                //_logger.Info($"SaveWebPdfs Inside Parallel!!");
                                //_logger.Info($"downloadPath: {pdf.downloadPath}");
                                //_logger.Info($"outputfilepath: {pdf.outputfilepath}");
                                //_logger.Error(e);
                                //_logger.Info($"SaveWebPdfs Inside Parallel!! end");
                                //rethrow exception
                                //throw;  
                                //throw exception only stop part of Parallel, other can still run
                                //should not throw, not throw-> other pdf can still download
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        //_logger.Info($"SaveWebPdfs Inside task!!");
                        //_logger.Error(e);
                        //_logger.Info($"SaveWebPdfs Inside task!! end");
                        //rethrow exception
                        throw;  //will not throw exception outside task and be catch
                    }
                });
            }
            catch (Exception e)
            {
                //cannot be catch if Inside task throw exception
                //_logger.Info($"SaveWebPdfs fail!!");
                //_logger.Error(e);
                //rethrow exception
                throw;
            }
        }
    }
}