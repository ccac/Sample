using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artech.WCFService.Service;
using Artech.WCFService.Contract;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Artech.WCFService.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            //HostingServiceViaCode();
            //HostingServiceViaConfig();
            //HostingDuplexServiceViaConfig();
            HostingDuplexServiceViaCode();
        }

        /*
         WCF中，Client端和Service端是通过Endpoint来通信的，Endpoint有包含３个部分，经典地称为ABC. 
         A代表Address，它包含一个URI，它指明Service存在于网络的某个地方，也就是说它为Client断指明在什么地方去找到这个Service。
         B代表Binding，Binding封装了所有Client和Service段消息交换的通信细节。
         C　代表Contract
         */
        static void HostingServiceViaCode()
        {
            //Specify the base Address
            Uri baseUri = new Uri("http://localhost:8080/calculatorService");

            //create a new ServiceHost object and specify the corresponding Service and base Address
            //It is recommanded to apply the using pattern to make sure the service host can be closed properly
            using (ServiceHost calculatorServiceHost = new ServiceHost(typeof(CalculatorService), baseUri))
            {
                //create a Binding for Endpoint
                BasicHttpBinding Binding = new BasicHttpBinding();
                                                
                //Create a Service Endpoint by specify the Address
                //(it is absolute or relative path based on the base Address,the empty string indicates the Address equals base Address)

                //Binding(the basicHttpBinding created) and Contrace(it is now the type info of the contract interface)
                calculatorServiceHost.AddServiceEndpoint(typeof(ICalculator), Binding, string.Empty);

                //Such a segment of code snip shows how to make the metadata exposed to outer world by setting the Service metadata behavior
                //Find the Service metadata behavior if exists,otherwize return null.
                ServiceMetadataBehavior behavior = calculatorServiceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

                //If the Service metadata behavior has not to added to the Service. 
                //we will create a new one and evaluate the HttpGetEnabled&HttpGetUrl to make outer world can retrieve to metadata.
                if (behavior == null)
                {
                    behavior = new ServiceMetadataBehavior();

                    behavior.HttpGetEnabled = true;
                    //HttpGetUrl is absolute or relative based on base Address
                    behavior.HttpGetUrl = baseUri;

                    //We must add the new behavior created to the behavior collection, otherwize it will never take effect.
                    calculatorServiceHost.Description.Behaviors.Add(behavior);
                }
                // If the metadata behavior exists in the behavior collection, we just need to evaluate the HttpGetEnabled&HttpGetUrl
                else
                {
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = baseUri;
                }

                //Add the opened event handler to make a friendly message diaplayed after opening the Service host indicating the Service begin to listen to request from Clients
                calculatorServiceHost.Opened += delegate
                {
                    Console.WriteLine("Calculator Service begin to listen via the Address:{0}",calculatorServiceHost.BaseAddresses[0].ToString());
                };

                //Open the Service host make it begin to listen to the Clients.
                calculatorServiceHost.Open();

                Console.ReadLine();
            }
        }

        static void HostingServiceViaConfig()
        {
            using (ServiceHost calculatorServiceHost = new ServiceHost(typeof(CalculatorService)))
            {
                calculatorServiceHost.Opened += delegate
                {
                    Console.WriteLine("Calculator Service begin to listen via the Address:{0}", calculatorServiceHost.BaseAddresses[0].ToString());
                };

                calculatorServiceHost.Open();

                Console.ReadLine();
            }
        }

        static void HostingDuplexServiceViaConfig()
        {
            using (ServiceHost calculatorServiceHost = new ServiceHost(typeof(DuplexCalculatorService)))
            {
                calculatorServiceHost.Opened += delegate
                {
                    Console.WriteLine("DuplexCalculator Service begin to listen via the Address:{0}", calculatorServiceHost.BaseAddresses[0].ToString());
                };

                calculatorServiceHost.Open();

                Console.ReadLine();
            }
        }

        static void HostingDuplexServiceViaCode()
        {
            Uri duplexUri = new Uri("http://localhost:9997/WCFService/DuplexCalculatorService");
            Uri tcpUri = new Uri("net.tcp://localhost:9996/WCFService/DuplexCalculatorService");

            using (ServiceHost calculatorServiceHost = new ServiceHost(typeof(DuplexCalculatorService), duplexUri, tcpUri))
            {
                WSDualHttpBinding wsdualBinding = new WSDualHttpBinding();
                NetTcpBinding tcpBinding = new NetTcpBinding();

                calculatorServiceHost.AddServiceEndpoint(typeof(IDuplexCalculator), wsdualBinding, string.Empty);
                calculatorServiceHost.AddServiceEndpoint(typeof(IDuplexCalculator), tcpBinding, string.Empty);

                ServiceMetadataBehavior behavior = calculatorServiceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

                if (behavior == null)
                {
                    behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    calculatorServiceHost.Description.Behaviors.Add(behavior);
                }
                else
                {
                    behavior.HttpGetEnabled = true;
                }

                calculatorServiceHost.Opened += delegate
                {
                    Console.WriteLine("DuplexCalculator Service begin to listen via the Address:{0}", calculatorServiceHost.BaseAddresses[0].ToString());
                };

                calculatorServiceHost.Open();

                Console.ReadLine();
            }
        }
    }
}
