using System.Net;
using System.Text;
using WebScraping;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.WriteLine("Dame el link del capitulo base ୧༼ಠ益ಠ༽︻╦╤─");
        var Url = Console.ReadLine();
        if (Url == null)
            Console.WriteLine("Link invalido ୧༼ಠ益ಠ༽୨");

        var ImgsPath = "./Images/";
        if (!Path.Exists(ImgsPath))
            Directory.CreateDirectory(ImgsPath);

        var PdfsPath = "./Pdfs/";
        if (!Path.Exists(PdfsPath))
            Directory.CreateDirectory(PdfsPath);

        while (true)
        {
            Console.Clear();
            await Scraping.DownloadImgsAsync(Url,ImgsPath);
            Scraping.CreatePDF(ImgsPath,PdfsPath,Url);
            Scraping.DeleteImgs(ImgsPath);
            Console.Clear();
            Console.WriteLine("Siguiente Capitulo (Ծ‸ Ծ)?\n(s/n)");
            var Eleccion = Console.ReadLine() ?? "";
            if (Eleccion.ToLower() != "s")
                break;
            Console.WriteLine("Seguiremos trabajando ε/̵͇̿̿/’̿’̿ ̿(◡︵◡)");
            Thread.Sleep(2500);
            Url = Scraping.NextLink(Url);
        }
        Console.Clear();
        Console.WriteLine("Hasta pronto ( ´◔ ω◔`) ノシ");
        Thread.Sleep(3000);
    }
}
