using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonsXML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter summ in KZT: ");
            double amount = double.Parse(Console.ReadLine());

            Console.WriteLine("Summ in USD: "+amount/GetRates()[2].discreaption);
            Console.WriteLine("\nSumm in EUR: " + amount / GetRates()[1].discreaption);
            Console.WriteLine();
            foreach (var item in GetRates())
            {
                Console.WriteLine(item);
            }

        }
        static void CreateShortVersion()
        {
            XmlDocument doc = new XmlDocument();
            //Корневой элемент
            XmlElement root = doc.CreateElement("Orders");
            //
            XmlElement xmlOrder = doc.CreateElement("order");
            XmlElement price = doc.CreateElement("price");
            //</price>
            price.InnerText = "3251";
            //<price>3251</price>
            XmlAttribute currency = doc.CreateAttribute("currency");
            currency.InnerText = "KZT";
            price.Attributes.Append(currency);
            //<price currency="KZT">3251</price>
            XmlAttribute discount = doc.CreateAttribute("discount");
            discount.InnerText = "2";
            price.Attributes.Append(discount);
            //<price currency="KZT" discount="2">3251</price>

            xmlOrder.AppendChild(price);
            root.AppendChild(xmlOrder);
            doc.AppendChild(root);
            doc.Save("orders.xml");
            /*
              <Orders>
                 <order>
                     <price discount="2" currency="KZT">3251</price>
                 </order>
              </Orders>
           */
            Console.WriteLine("Good");
        }

        static XmlDocument GetExchangeRates()
        {
            XmlDocument shortRates = new XmlDocument();
            XmlElement root = shortRates.CreateElement("rates");

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load("http://www.nationalbank.kz/rss/rates.xml");
                if (doc.HasChildNodes)
                {
                    foreach (XmlNode item in doc.ChildNodes[1])
                    {
                        if (item.HasChildNodes)
                        {///1
                            //foreach (XmlNode chanel in item.ChildNodes)
                            //{
                            //    Console.WriteLine("\t{0}",chanel.Name);
                            //    if (chanel.Name.Equals("item"))
                            //    {
                            //        foreach (XmlNode chitem in chanel.ChildNodes)
                            //        {
                            //            Console.WriteLine("\t\t{0}",chitem.Name);
                            //        }
                            //    }
                            //}
                            ///2
                            //foreach (XmlNode rootItam in doc.SelectNodes("rss/channel/item"))
                            //{
                            //    string rateName = rootItam.SelectSingleNode("title").InnerText;
                            //    string rateDescrip = rootItam.SelectSingleNode("description").InnerText;

                            //    XmlElement rate = shortRates.CreateElement(rateName);
                            //    rate.InnerText = rateDescrip;

                            //    XmlAttribute pubDateAtr = shortRates.CreateAttribute("pubDate");
                            //    pubDateAtr.InnerText=rootItam.SelectSingleNode("pubDate").InnerText;
                            //    rate.Attributes.Append(pubDateAtr);
                            //    root.AppendChild(rate);
                            //}
                            //shortRates.AppendChild(root);
                            //shortRates.Save("shortRate.xml");

                        }

                    }

                }
            }
            catch (Exception)
            {

            }
            return doc;
        }
        static List<rate> GetRates()
        {
            List<rate> rates = new List<rate>();
            //foreach (XmlNode root in doc.SelectNodes("rss/channel/item"))
            //{

            //}
            foreach (XmlNode root in GetExchangeRates().SelectNodes("rss/channel/item"))
            {
                rate rate = new rate();
                rate.title = root.SelectSingleNode("title").InnerText;
                rate.pubDate = DateTime.Parse(root.SelectSingleNode("pubDate").InnerText);
                rate.discreaption = double.Parse(root.SelectSingleNode("description").InnerText.Replace(".", ","));
                rates.Add(rate);
            }

            return rates;
        }
        public class rate
        {
            public string title { get; set; }
            public DateTime pubDate { get; set; }
            public double discreaption { get; set; }
            public override string ToString()
            {
                string str = string.Format("{0} - {1}", title, discreaption);
                return str;
            }
        }
    }
}
