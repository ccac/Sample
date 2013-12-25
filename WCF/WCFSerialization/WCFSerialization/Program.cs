using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace WCFSerialization
{
    class Program
    {
        static string _basePath = @"D:\MyWork\Sample\WCF\WCFSerialization\";

        static void Main(string[] args)
        {
            SerializeViaDataContractSerializer();
            DeserializeViaDataContractSerializer();
        }

        static void SerializeViaDataContractSerializer()
        {
            DataContractProduct product = new DataContractProduct(Guid.NewGuid(), "Dell PC", "Xiamen FuJian", 4500);
            DataContractOrder order = new DataContractOrder(Guid.NewGuid(), DateTime.Today, product, 300);
            string fileName = _basePath + "Order.DataContractSerializer.xml";
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DataContractOrder));
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs))
                {
                    serializer.WriteObject(writer, order);
                }
            }
            Process.Start(fileName);
        }


        static void DeserializeViaDataContractSerializer()
        {
            string fileName = _basePath + "Order.DataContractSerializer.xml";
            DataContractOrder order;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DataContractOrder));
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                {
                    order = serializer.ReadObject(reader) as DataContractOrder;
                }
            }

            Console.WriteLine(order);
            Console.Read();
        }
    }
}
