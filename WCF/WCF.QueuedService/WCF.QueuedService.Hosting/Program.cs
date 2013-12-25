using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Messaging;
using WCF.QueuedService.Service;

namespace WCF.QueuedService.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @".\private$\orders";
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path, true);
            }

            using (ServiceHost host = new ServiceHost(typeof(OrderProcessService)))
            {
                host.Opened += delegate
                {
                    Console.WriteLine("Service has begun to listen\n\n");
                };

                host.Open();

                Console.Read();
            }

        }
    }
}
