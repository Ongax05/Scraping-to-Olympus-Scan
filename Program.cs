using System;
using WebScraping;

class Program
{
    static void Main(string[] args)
    {
        var Url = "https://olympusv2.gg/capitulo/39894/comic-helmut-el-nino-abandonado";
        var Links = Scraping.GetLinks(Url);
    }
}
