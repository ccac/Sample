using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class Program
    {
        static void Main()
        {
            try
            {
                //InvocateCalclatorServiceViaCode();

                InvocateCalclatorServiceViaConfiguration();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();    
        }

        static void InvocateCalclatorServiceViaCode()
        {
            Binding httpBinding = new BasicHttpBinding();
            Binding tcpBinding = new NetTcpBinding();

            EndpointAddress httpAddress = new EndpointAddress("http://localhost:8888/generalCalculator");
            EndpointAddress tcpAddress = new EndpointAddress("net.tcp://localhost:9999/generalCalculator");
            EndpointAddress httpAddress_iisHost = new EndpointAddress("http://localhost/wcfservice/GeneralCalculatorService.svc");

            Console.WriteLine("Invocate self-host calculator service... ...");

            #region Invocate Self-host service
            using (GeneralCalculatorClient calculator_http = new GeneralCalculatorClient(httpBinding, httpAddress))
            {
                using (GeneralCalculatorClient calculator_tcp = new GeneralCalculatorClient(tcpBinding, tcpAddress))
                {
                    try
                    {
                        Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_http.Add(1, 2));

                        Console.WriteLine("Begin to invocate calculator service via tcp transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_tcp.Add(1, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            #endregion

            Console.WriteLine("\n\nInvocate IIS-host calculator service... ...");

            #region Invocate IIS-host service
            using (GeneralCalculatorClient calculator = new GeneralCalculatorClient(httpBinding, httpAddress_iisHost))
            {
                try
                {
                    Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                    Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator.Add(1, 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }

        static void InvocateCalclatorServiceViaConfiguration()
        {
            Console.WriteLine("Invocate self-host calculator service... ...");

            #region Invocate Self-host service
            using (GeneralCalculatorClient calculator_http = new GeneralCalculatorClient("selfHostEndpoint_http"))
            {
                using (GeneralCalculatorClient calculator_tcp = new GeneralCalculatorClient("selfHostEndpoint_tcp"))
                {
                    try
                    {
                        Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_http.Add(1, 2));

                        Console.WriteLine("Begin to invocate calculator service via tcp transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_tcp.Add(1, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            #endregion

            Console.WriteLine("\n\nInvocate IIS-host calculator service... ...");

            #region Invocate IIS-host service
            using (GeneralCalculatorClient calculator = new GeneralCalculatorClient("iisHostEndpoint"))
            {
                try
                {
                    Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                    Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator.Add(1, 2));                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }
    }
}
