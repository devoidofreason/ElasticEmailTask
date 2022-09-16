using System;
using System.Linq;
using System.Net;

namespace Resolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InputManager inputManager = new InputManager(args);

            inputManager.getErrorMessages().ForEach(error =>
            {
                Console.WriteLine(error);
            });

            inputManager.getDomains().ForEach(domain =>
            {
                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(domain);
                    Console.WriteLine("\nDomain {0} refers to IP adresses:", domain);
                    foreach (IPAddress ip in entry.AddressList)
                    {
                        Console.Write("\t{0}", ip.ToString());
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("\nDomain {0} entry attempt caused an exception.\n{1}", domain, ex.Message);

                }
            });
        }
    }
}
