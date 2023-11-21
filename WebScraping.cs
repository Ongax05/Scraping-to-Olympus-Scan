using System.Text.RegularExpressions;
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
            string Nombre = Img1[..^8];
            var Links = imgNodes.Select(n => n.GetAttributeValue("src", ""));
            var sortedLinks = Links.Where(l => l.Contains(Nombre));
            return sortedLinks.ToList();
        }

        public static string NextLink(string url)
        {
            var document = Web.Load(url);
            var NextCapLink = document.DocumentNode
                .SelectNodes("//a")
                .Where(n => n.GetAttributeValue("name", "").ToString() == "capitulo siguiente").FirstOrDefault();
            return "https://olympusv2.gg" + NextCapLink.GetAttributeValue("href","");
        }
    }
}
