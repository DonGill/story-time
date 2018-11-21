using System;
using System.Xml;
using System.Configuration;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Flippy
{
    class Program
    {
        static void Main(string[] args)
        {
            //temp list<string> of mp3 urls 
            List<string> tempListing = new List<string>();
            tempListing.Add("https://archive.org/download/otr_cbsradiomysterytheater/CBSRadioMysteryTheaterTheSmokingPistolDec141976.mp3");
            tempListing.Add("https://archive.org/download/otr_cbsradiomysterytheater/CBSRMT740106-000132-TheOldOnesAreHardtoKill.mp3");

            //generate a playlist from the temp listing
            generate_playlist(tempListing);
            play_it();
            
            //load stations from the app.config file
            //parse_page(ConfigurationSettings.AppSettings["cbsradiomystery"]);
        }

        static void generate_playlist(List<string> episodes)
        {
            //check to see if 'story-spin-temp.wpl' file does not exist.
            //if it does - delete it.
            //if(System.IO.File.Exists(""))
            
            //create a base wpl formatted xml document
            XmlDocument xml_doc = new XmlDocument();
            XmlNode xml_root = xml_doc.CreateElement("smil");
            xml_doc.AppendChild(xml_root);

            XmlNode xml_head = xml_doc.CreateElement("head");
            xml_root.AppendChild(xml_head);

            XmlNode xml_body = xml_doc.CreateElement("body");
            xml_root.AppendChild(xml_body);

            XmlNode xml_seq = xml_doc.CreateElement("seq");
            xml_body.AppendChild(xml_seq);

            XmlNode xml_media1 = xml_doc.CreateElement("media");
            XmlAttribute xml_media1_src = xml_doc.CreateAttribute("src");
            xml_media1_src.Value = "https://archive.org/download/otr_cbsradiomysterytheater/CBSRadioMysteryTheaterTheSmokingPistolDec141976.mp3";
            xml_media1.Attributes.Append(xml_media1_src);
            xml_seq.AppendChild(xml_media1);


            XmlNode xml_media2 = xml_doc.CreateElement("media");
            XmlAttribute xml_media2_src = xml_doc.CreateAttribute("src");
            xml_media2_src.Value = "https://archive.org/download/otr_cbsradiomysterytheater/CBSRMT740106-000132-TheOldOnesAreHardtoKill.mp3";
            xml_media2.Attributes.Append(xml_media2_src);
            xml_seq.AppendChild(xml_media2);

            xml_doc.Save("story-spin-temp.wpl");
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
               // Console.WriteLine(href);
               // play_it(href);
            }
        }

        static void play_it()
        {
            string ex_loc = System.IO.Directory.GetCurrentDirectory();
            Process.Start("wmplayer", ex_loc + @"\story-spin-temp.wpl");
            //Process.Start("microsoft-edge:" + url);
            //Console.WriteLine(ex_loc);
            

            //Process.Start(@"C:\Program Files (x86)\Windows Media Player\wmplayer.exe", url);
            //Console.ReadLine();

        }
    }
}
