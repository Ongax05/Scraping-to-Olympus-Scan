using HtmlAgilityPack;
using ImageMagick;

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
            try
            {
                Console.Clear();
                Console.WriteLine("Descargando imagenes ...");
                var Client = new HttpClient();

                var Links = GetLinks(url);

                for (int i = 0; i < Links.Count; i++)
                {
                    var uri = new Uri(Links[i]);
                    var fileName = $"{ImgsPath}{i}.jpg";

                    var response = await Client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        using var stream = new FileStream(fileName, FileMode.Create);
                        await response.Content.CopyToAsync(stream);
                    }
                }
                Console.Clear();
                Console.WriteLine("Imagenes descargadas correctamente ( ˘ ³˘)♥");
                Thread.Sleep(2500);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.Error.WriteLine($"Algo salio mal ಥ﹏ಥ\nError: {ex}");
            }
        }

        public static List<string> GetImgNames(string ImgsPath)
        {
            var Directory = new DirectoryInfo(ImgsPath);
            var r = Directory
                .GetFiles()
                .OrderBy(i => int.Parse(i.Name[..^4]))
                .Select(i => i.FullName)
                .ToList();
            return r;
        }

        public static void DeleteImgs(string ImgsPath)
        {
            var Imgs = GetImgNames(ImgsPath);
            foreach (string Img in Imgs)
            {
                File.Delete(Img);
            }
        }

        public static string GetTitle(string url)
        {
            var document = Web.Load(url);
            var PagTitle = document.DocumentNode.SelectSingleNode("//title").InnerHtml.ToString();
            var TitleSplit = PagTitle.Split(" ").ToList();
            var Num = TitleSplit[1];
            TitleSplit.RemoveRange(0, 3);
            TitleSplit.RemoveRange(TitleSplit.Count - 3, 3);
            var Title = $"{string.Join(" ", TitleSplit)} {Num}.pdf";
            return Title;
        }

        public static void CreatePDF(string ImgsPath, string PdfsPath, string Url)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Creando Pdf ...");
                var Imgs = GetImgNames(ImgsPath);
                var PdfImages = new MagickImageCollection();
                foreach (var img in Imgs)
                {
                    PdfImages.Add(img);
                }
                string Title = GetTitle(Url);
                PdfImages.Write($"{PdfsPath + Title}");
                Console.Clear();
                Console.WriteLine("Pdf creado correctamente ᕦ(ò_óˇ)ᕤ");
                Thread.Sleep(2500);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.Error.WriteLine($"Algo salio mal ಥ﹏ಥ\nError: {ex}");
            }
        }
    }
}
