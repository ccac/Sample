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
                //InvocateCalclatorServiceViaConfiguration();
                //InvocateDuplexCalculator();
                InvocateTcpDuplexCalculator();
                //InvocateSessionfulCalculator();
                //InvocateDivideCalculator();
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

        static void InvocateDuplexCalculator()
        { 
            CalculatorCallbackHandler callbackHandler = new CalculatorCallbackHandler();

            using(DuplexCalculatorClient calculator = new DuplexCalculatorClient(new InstanceContext(callbackHandler)))
            {
                Console.WriteLine("Begin to invocate duplex calculator... ...");
                calculator.Add(1, 2);
            }
        }

        static void InvocateTcpDuplexCalculator()
        {
            DuplexChannelFactory<ITcpDuplexCalculator> factory = new DuplexChannelFactory<ITcpDuplexCalculator>(
                new InstanceContext(new CallBack()),
                "tcpDuplexEndpoint");

            ITcpDuplexCalculator calculator = factory.CreateChannel();

            Console.WriteLine("Try to invoke the Add method");

            try
            {
                calculator.Add(1, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception is thrown!\n\t:Type:{0}\n\tMessage:{1}", ex.GetType(), ex.Message);
            }

            Console.Read();

        }

        static void InvocateSessionfulCalculator()
        {
            //using (SessionfulCalculatorClient calculator = new SessionfulCalculatorClient())
            //{
            //    Console.WriteLine("Begin to invocate sessionful calculator... ...");
            //    Console.WriteLine("Call the method Add (2)");
            //    calculator.Add(2);
            //    Console.WriteLine("Call the method Add (3)");
            //    calculator.Add(3);
            //    Console.WriteLine("The result is {0}", calculator.GetResult());
            //}

            ChannelFactory<ISessionfulCalculator> calculatorChannelFactory = new ChannelFactory<ISessionfulCalculator>("httpEndpoint");
            Console.WriteLine("Create a calculator proxy: proxy1");
            ISessionfulCalculator proxy1 = calculatorChannelFactory.CreateChannel();
            Console.WriteLine("Invocate  proxy1.Adds(1)");
            proxy1.Add(1);
            Console.WriteLine("Invocate  proxy1.Adds(2)");
            proxy1.Add(2);
            Console.WriteLine("The result return via proxy1.GetResult() is : {0}", proxy1.GetResult());

            Console.WriteLine("Create a calculator proxy: proxy2");
            ISessionfulCalculator proxy2 = calculatorChannelFactory.CreateChannel();
            Console.WriteLine("Invocate  proxy2.Adds(1)");
            proxy2.Add(1);
            Console.WriteLine("Invocate  proxy2.Adds(2)");
            proxy2.Add(2);
            Console.WriteLine("The result return via proxy2.GetResult() is : {0}", proxy2.GetResult());

            Console.Read();

        }


        static void InvocateDivideCalculator()
        {
            ChannelFactory<IDivideCalculator> calculatorFactory = new ChannelFactory<IDivideCalculator>("divideEndpoint");
            IDivideCalculator calculator = calculatorFactory.CreateChannel();
            try
            {
                Console.WriteLine("Try to invoke Divide method");
                Console.WriteLine("x / y =  {2} when x = {0} and y = {1}", 2, 0, calculator.Divide(2, 0));
            }
            catch (FaultException<MathError> ex)
            {
                MathError error = ex.Detail;
                Console.WriteLine("An Fault is thrown.\n\tFault code:{0}\n\tFault Reason:{1}\n\tOperation:{2}\n\tMessage:{3}", ex.Code, ex.Reason, error.Operation, error.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception is thrown.\n\tException Type:{0}\n\tError Message:{1}", ex.GetType(), ex.Message);
            }
            Console.Read();

            //DivideCalculatorClient client = new DivideCalculatorClient();
            //try
            //{
            //    Console.WriteLine("Try to invoke Divide method");
            //    Console.WriteLine("x / y =  {2} when x = {0} and y = {1}", 2, 0, client.Divide(2, 0));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An Exception is thrown.\n\tException Type:{0}\n\tError Message:{1}", ex.GetType(), ex.Message);
            //}
            //Console.Read();
        }
    }
}
