using System;
using System.Xml;
using System.Configuration;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


namespace Flippy
{
    class Program
    {
        static void Main(string[] args)
        {
            //generate a playlist from the temp listing
            generate_playlist(refetch_media_sources());

            //randomize and play
            play_it();
        }

        static void generate_playlist(List<string> media_urls)
        {
           
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

            foreach (var media_url in media_urls)
            {
                XmlNode xml_media = xml_doc.CreateElement("media");
                XmlAttribute xml_media_src = xml_doc.CreateAttribute("src");
                xml_media_src.Value = media_url;
                xml_media.Attributes.Append(xml_media_src);
                xml_seq.AppendChild(xml_media);
            }
            xml_doc.Save("story-spin-temp.wpl");
        }

        static List<string> refetch_media_sources()
        {
            List<string> media_urls = new List<string>();

            foreach(string station_source in ConfigurationManager.AppSettings)
            { 
                //string media_src = ConfigurationManager.AppSettings["cbsradiomystery"];
                HtmlDocument doc = new HtmlWeb().Load(ConfigurationManager.AppSettings[station_source]);

                var items = doc.DocumentNode.SelectNodes("//link[@itemprop = 'associatedMedia']");
                foreach (var item in items)
                {
                    string media_url = item.Attributes["href"].Value;
                    media_urls.Add(media_url);
                }
            }

            //shuffle the order (psuedo random) and return
            Random rnd = new Random();
            return media_urls.OrderBy(x => rnd.Next()).ToList(); 
        }

        static void play_it()
        {
            string ex_loc = System.IO.Directory.GetCurrentDirectory();
            Process.Start("wmplayer", ex_loc + @"\story-spin-temp.wpl");
        }
    }
}
