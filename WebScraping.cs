using System.IO.Compression;
using HtmlAgilityPack;

namespace WebScraping
{
    public class Scraping
    {
        private static readonly HtmlWeb Web = new();

        public static List<string> GetLinks(string url)
        {
            var document = Web.Load(url);
            var imgNodes = document.DocumentNode.SelectNodes(
                "//img[contains(@class, 'object-cover rounded-inherit w-full h-full')]"
            );
            string Img1 = imgNodes.First().GetAttributeValue("src", "").ToString();
            string Nombre = Img1[..^16];
            var Links = imgNodes.Select(n => n.GetAttributeValue("src", ""));
            var sortedLinks = Links.Where(l => l.Contains(Nombre));
            return sortedLinks.ToList();
        }

        public static string NextLink(string url)
        {
            var document = Web.Load(url);
            var NextCapLink = document.DocumentNode
                .SelectNodes("//a")
                .Where(n => n.GetAttributeValue("name", "").ToString() == "capitulo siguiente")
                .FirstOrDefault();
            return "https://olympusvisor.com" + NextCapLink.GetAttributeValue("href", "");
        }

        public static async Task<List<byte[]>> DownloadImgsAsync(string url)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Downloading images ...");
                var Client = new HttpClient();
                List<byte[]> ImagesList = new();
                var Links = GetLinks(url);

                for (int i = 0; i < Links.Count; i++)
                {
                    var uri = new Uri(Links[i]);
                    var response = await Client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var Image = await response.Content.ReadAsByteArrayAsync();
                        ImagesList.Add(Image);
                    }
                }
                Console.Clear();
                Console.WriteLine("Images downloaded successfully");
                Thread.Sleep(2500);
                return ImagesList;
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.Error.WriteLine($"Something went wrong ಥ﹏ಥ\nError: {ex}");
                Thread.Sleep(10000);
                throw;
            }
        }

        public static void Zipper(
            string OutputPath,
            string Url,
            string Extension,
            List<byte[]> Images
        )
        {
            try
            {
                string Title = GetTitle(Url);
                string zipName = $"{OutputPath}/{Title}.{Extension}";
                Console.WriteLine($"Zipping: {zipName}");
                using FileStream fs = new(zipName, FileMode.Create);
                using ZipArchive archive = new(fs, ZipArchiveMode.Create);
                for (int i = 0; i < Images.Count; i++)
                {
                    var Num = i < 10 ? $"0{i}" : $"{i}";
                    var entryName = $"{Num}.jpg";
                    ZipArchiveEntry entry = archive.CreateEntry(entryName);
                    using Stream entryStream = entry.Open();
                    entryStream.Write(Images[i], 0, Images[i].Length);
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.Error.WriteLine($"Something went wrong ಥ﹏ಥ\nError: {ex}");
                Thread.Sleep(10000);
                throw;
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
            var Title = $"{string.Join(" ", TitleSplit)} {Num}";
            return Title;
        }
    }
}
