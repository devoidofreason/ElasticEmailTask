using System;
using System.Collections.Generic;

namespace Resolver
{
    internal class InputManager
    {
        private const string inputSwitch = "-input";
        private const string dnsSwitch = "-dns";
        private const string outputSwitch = "-output";
        private const string fileReadError = "Attempt on read from file: {0} caused an exception : {1}";

        private string dns = string.Empty;
        private string outputFilePath = string.Empty;
        private List<string> domains = new List<string>();
        private List<string> errorMessages = new List<string>();

        public InputManager(string[] args)
        {
            bool isInput = false;
            bool isDns = false;
            bool isOutput = false;
            foreach (string arg in args)
            {
                if (isInput)
                {
                    try
                    {
                        foreach(string line in System.IO.File.ReadAllLines(arg))
                        {
                            domains.Add(line);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(string.Format(fileReadError, arg, ex.Message));
                    }
                    isInput = false;
                    continue;
                }

                if(isDns)
                {
                    dns = arg;
                    isDns = false;
                    continue;
                }

                if(isOutput)
                {
                    outputFilePath = arg;
                    isOutput = false;
                    continue;
                }

                switch (arg)
                {
                    case inputSwitch:
                        isInput = true;
                        continue;
                    case dnsSwitch:
                        isDns = true;
                        continue;
                    case outputSwitch:
                        isOutput = true;
                        continue;
                    default:
                        domains.Add(arg);
                        break;
                }
            }
        }

        public List<string> getDomains() => domains;

        public List<string> getErrorMessages() => errorMessages;
    }
}
