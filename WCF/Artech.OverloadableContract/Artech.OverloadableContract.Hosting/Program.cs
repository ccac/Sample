using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Artech.OverloadableContract.Service;

namespace Artech.OverloadableContract.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CalculatorService)))
            {
                host.Open();
                Console.WriteLine("Calculator service has begun to listen... ...");
                Console.Read();
            }
        }
    }
}
