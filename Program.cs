﻿using System.Net;
using WebScraping;

class Program
{
    static void Main(string[] args)
    {
//        var Url = "https://olympusv2.gg/capitulo/39894/comic-helmut-el-nino-abandonado";

        var ImgsPath = "./Images/";
        if (!Path.Exists(ImgsPath))
            Directory.CreateDirectory(ImgsPath);

        var PdfsPath = "./Pdfs/";
        if (!Path.Exists(PdfsPath))
            Directory.CreateDirectory(PdfsPath);

//        await Scraping.DownloadImgsAsync(Url, ImgsPath);
        Scraping.CreatePDF(ImgsPath, PdfsPath, "cap1.pdf");
    }
}
