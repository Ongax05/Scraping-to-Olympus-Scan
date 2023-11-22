using HtmlAgilityPack;
using Aspose.Pdf;
using Aspose.Imaging;

namespace WebScraping
{
    public class Scraping
    {
        private static readonly HtmlWeb Web = new();

        public static List<string> GetLinks(string url)
        {
            var document = Web.Load(url);
            var imgNodes = document
                .DocumentNode
                .SelectNodes(
                    "//img[contains(@class, 'object-cover rounded-inherit w-full h-full')]"
                );
            string Img1 = imgNodes.First().GetAttributeValue("src", "").ToString();
            string Nombre = Img1[..^8];
            var Links = imgNodes.Select(n => n.GetAttributeValue("src", ""));
            var sortedLinks = Links.Where(l => l.Contains(Nombre));
            return sortedLinks.ToList();
        }

        public static string NextLink(string url)
        {
            var document = Web.Load(url);
            var NextCapLink = document
                .DocumentNode
                .SelectNodes("//a")
                .Where(n => n.GetAttributeValue("name", "").ToString() == "capitulo siguiente")
                .FirstOrDefault();
            return "https://olympusv2.gg" + NextCapLink.GetAttributeValue("href", "");
        }

        public static async Task DownloadImgsAsync(string url, string ImgsPath)
        {
            var Client = new HttpClient();

            var Links = GetLinks(url);

            for (int i = 0; i < Links.Count; i++)
            {
                var uri = new Uri(Links[i]);
                var fileName = $"{ImgsPath}{i}.jpg";

                var response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    //using var stream = ConvertImageToJpeg(new FileStream(fileName, FileMode.Create));
                    //await response.Content.CopyToAsync(stream);
                }
            }
        }

        public static List<string> GetImgNames(string ImgsPath)
        {
            var Directory = new DirectoryInfo(ImgsPath);
            var r = Directory.GetFiles().Select(f => f.Name).OrderBy(i=> int.Parse(i[..^4])).ToList();
            return r;   
        }

        public static void CreatePDF (string ImgsPath, string PdfsPath, string PdfName)
        {
            Document pdfDocument = new();
            var Imgs = GetImgNames(ImgsPath);
            for (int i = 0; i < Imgs.Count; i++)
            {
                FileStream ImageStream = new(ImgsPath + Imgs[i], FileMode.Open);
                pdfDocument.Pages.Add();
                pdfDocument.Pages[i+1].Resources.Images.Add(ImageStream);
            }
            pdfDocument.Save(PdfsPath + PdfName);
        }
    }
}
