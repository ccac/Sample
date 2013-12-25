using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //    using (CalculatorClient calculator = new CalculatorClient())
            //    {
            //        Console.WriteLine("Begin to invocate the calculator Servcie");

            //        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 
            //            1, 
            //            2,
            //            calculator.Add(1, 2));

            //        Console.ReadLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("StackTrace:{0}", ex.StackTrace);
            //    Console.WriteLine("Message:{0}", ex.Message);
            //    Console.ReadLine();
            //    //throw;
            //}

            InvokeDuplexCalculator();
        }

        static void InvokeDuplexCalculator()
        {
            try
            {
                CalculatorCallback callbackhandle = new CalculatorCallback();

                using (DuplexCalculatorClient calculator = new DuplexCalculatorClient(new InstanceContext(callbackhandle), "duplexCalculatorEndPoint"))
                {
                    Console.WriteLine("Begin to invocate the duplex calculator http Servcie");

                    calculator.Add(1, 2);

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("StackTrace:{0}", ex.StackTrace);
                Console.WriteLine("Message:{0}", ex.Message);
                Console.ReadLine();
                //throw;
            }
        }
    }
}
