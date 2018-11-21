using System;
using System.Configuration;
using HtmlAgilityPack;
using System.Diagnostics;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Flippy
{
    class Program
    {
        static void Main(string[] args)
        {
            //load stations from the app.config file
            parse_page(ConfigurationSettings.AppSettings["cbsradiomystery"]);
        }

        static void parse_page(string urlToFetch)
        {
            Random rnd = new Random();
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(urlToFetch);

            var items = doc.DocumentNode.SelectNodes("//link[@itemprop = 'associatedMedia']");
            if (items.Count > 0)
            {
                HtmlNode item = items[rnd.Next(1, items.Count - 1)];
                string href = item.Attributes["href"].Value;
                Console.WriteLine(href);
                play_it(href);
            }
        }

        static void play_it(string url)
        {
            string ex_loc = System.IO.Directory.GetCurrentDirectory();
            //Process.Start("microsoft-edge:" + url);
            Console.WriteLine(ex_loc);
            Process.Start("wmplayer", ex_loc + @"\donkey-long.wpl");

            //Process.Start(@"C:\Program Files (x86)\Windows Media Player\wmplayer.exe", url);
            Console.ReadLine();

        }
    }
}
