using System.Net;
using System.Text;
using WebScraping;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.WriteLine("Give me the link to the base chapter ୧༼ಠ益ಠ༽︻╦╤─");
        var Url = Console.ReadLine();
        if (Url == null)
            Console.WriteLine("Invalid link ୧༼ಠ益ಠ༽୨");

        var ImgsPath = "./Images/";
        if (!Path.Exists(ImgsPath))
            Directory.CreateDirectory(ImgsPath);

        var PdfsPath = "./Pdfs/";
        if (!Path.Exists(PdfsPath))
            Directory.CreateDirectory(PdfsPath);

        var ZipsPath = "./Zips/";
        if (!Path.Exists(ZipsPath))
            Directory.CreateDirectory(ZipsPath);

        while (true)
        {
            Console.Clear();
            await Scraping.DownloadImgsAsync(Url,ImgsPath);
            Scraping.Zipper(ImgsPath,ZipsPath,Url);
            Scraping.DeleteImgs(ImgsPath);
            Console.Clear();
            Console.WriteLine("Next Chapter (Ծ‸ Ծ)?\n(s/n)");
            var Eleccion = Console.ReadLine() ?? "";
            if (Eleccion.ToLower() != "s")
                break;
            Console.WriteLine("We will continue to work ε/̵͇̿̿/’̿’̿ ̿(◡︵◡)");
            Thread.Sleep(3500);
            Url = Scraping.NextLink(Url);
        }
        Console.Clear();
        Console.WriteLine("See you soon ( ´◔ ω◔`) ノシ");
        Thread.Sleep(3000);
    }
}
