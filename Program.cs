using WebScraping;

class Program
{
    static async Task Main(string[] args)
    {
        bool Flag = true;
        bool SD = true;
        int NumCaps = 0;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.WriteLine("Provide the link to the base chapter");
        var Url = Console.ReadLine();
        if (Url == null)
            Console.WriteLine("Invalid link");

        var OutputPath = "./Output/";
        if (!Path.Exists(OutputPath))
            Directory.CreateDirectory(OutputPath);

        Console.Clear();
        Console.WriteLine("What type of file do you want?\n1.Zip\t2.cbz");
        string Opt = Console.ReadLine();

        if (Opt is null || !Opt.Equals("1") && !Opt.Equals("2"))
        {
            Flag = false;
            Console.Clear();
            Console.WriteLine("Invalid type");
            Thread.Sleep(3000);
        }

        Console.Clear();
        Console.WriteLine(
            "Choose download mode:\n1. Download all at once\n2. Download one by one"
        );
        string Opt2 = Console.ReadLine();

        if (Opt2 is null || !Opt2.Equals("1") && !Opt2.Equals("2"))
        {
            Flag = false;
            Console.Clear();
            Console.WriteLine("Invalid download mode");
            Thread.Sleep(3000);
        }
        else if (Opt2.Equals("1"))
        {
            Console.Clear();
            Console.WriteLine("Enter the number of chapters you wish to download");
            var NumCaps1 = Console.ReadLine();
            if (int.TryParse(NumCaps1, out int num))
            {
                SD = false;
                NumCaps += num;
                Console.Clear();
                Console.WriteLine($"{num} will be donwloaded");
            }
            else
            {
                Flag = false;
                Console.Clear();
                Console.WriteLine("Invalid number");
                Thread.Sleep(3000);
            }
        }

        int loops = 0;
        while (Flag)
        {
            Console.Clear();
            var Images = await Scraping.DownloadImgsAsync(Url);

            switch (Opt)
            {
                case "1":
                    Scraping.Zipper(OutputPath, Url, "zip", Images);
                    break;
                case "2":
                    Scraping.Zipper(OutputPath, Url, "cbz", Images);
                    break;
            }
            Console.Clear();
            if (SD)
            {
                Console.WriteLine("Next Chapter?\n(y/n)");
                var Eleccion = Console.ReadLine() ?? "";
                if (Eleccion.ToLower() != "y")
                    Flag = false;
                else
                {
                    Console.WriteLine("We will continue to work");
                    Thread.Sleep(3500);
                }
            } else
            {
                loops ++;
                if (loops == NumCaps)
                {
                    break;
                }
            }

            Url = Scraping.NextLink(Url);
        }
        Console.Clear();
        Console.WriteLine("See you soon");
        Thread.Sleep(3000);
    }
}
