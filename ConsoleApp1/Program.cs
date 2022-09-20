using System;

namespace Resolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InputManager inputManager = new InputManager(args);

            inputManager.GetErrorMessages().ForEach(error =>
            {
                Console.WriteLine(error);
            });

            IPResolver iPResolver = new IPResolver(inputManager.GetDomains(), inputManager.GetDns());

            OutputManager outputManager = new OutputManager(inputManager.GetDns(), inputManager.GetOutputPath(), iPResolver.GetReport());
            
            outputManager.WriteReport();
        }
    }
}
