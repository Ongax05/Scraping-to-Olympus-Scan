using System.Net;
using System.Text;
using WebScraping;

class Program
{
    static async Task Main(string[] args)
    {
        bool Flag = true;
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

        var CbzsPath = "./Cbzs/";
        if (!Path.Exists(CbzsPath))
            Directory.CreateDirectory(CbzsPath);

        Console.Clear();
        Console.WriteLine("What type of file do you want (Ծ‸ Ծ)?\n1.Pdf\t2.Zip\t3.cbz");
        string Opt = Console.ReadLine();

        if (Opt is null || !Opt.Equals("1") && !Opt.Equals("2") && !Opt.Equals("3"))
        {
            Flag = false;
            Console.Clear();
            Console.WriteLine("Invalid type ୧༼ಠ益ಠ༽୨");
            Thread.Sleep(3000);
        }

        while (Flag)
        {
            Console.Clear();
            await Scraping.DownloadImgsAsync(Url, ImgsPath);

            switch (Opt)
            {
                case "1":
                    Scraping.CreatePDF(ImgsPath, PdfsPath, Url);
                    break;
                case "2":
                    Scraping.Zipper(ImgsPath, ZipsPath, Url, "zip");
                    break;
                case "3":
                    Scraping.Zipper(ImgsPath, CbzsPath, Url, "cbz");
                    break;
            }
            Scraping.DeleteImgs(ImgsPath);
            Console.Clear();
            Console.WriteLine("Next Chapter (Ծ‸ Ծ)?\n(s/n)");
            var Eleccion = Console.ReadLine() ?? "";
            if (Eleccion.ToLower() != "s")
                Flag = false;
            else
            {
                Console.WriteLine("We will continue to work ε/̵͇̿̿/’̿’̿ ̿(◡︵◡)");
                Thread.Sleep(3500);
            }

            Url = Scraping.NextLink(Url);
        }
        Console.Clear();
        Console.WriteLine("See you soon ( ´◔ ω◔`) ノシ");
        Thread.Sleep(3000);
    }
}
