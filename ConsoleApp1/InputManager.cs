using System;
using System.Collections.Generic;
using System.Net;

namespace Resolver
{
    internal class InputManager
    {
        private const string inputSwitch = "-input";
        private const string dnsSwitch = "-dns";
        private const string outputSwitch = "-output";
        private const string fileReadError = "Attempt on read from file: {0} caused an exception : {1}";
        private const string ipAddressParseError = "Attempt on parse IP address from: {0} caused an exception : {1}";

        private IPAddress dns = default;
        private string outputFilePath = default;
        private List<string> domains = new List<string>();
        private List<string> errorMessages = new List<string>();

        public InputManager(string[] args)
        {
            var isInput = false;
            var isDns = false;
            var isOutput = false;
            foreach (string arg in args)
            {
                if (isInput)
                {
                    try
                    {
                        foreach(var line in System.IO.File.ReadAllLines(arg))
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

                if (isDns)
                {
                    try
                    {
                        dns = IPAddress.Parse(arg);
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(string.Format(ipAddressParseError, arg, ex.Message));
                    }
                    isDns = false;
                    continue;
                }

                if (isOutput)
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

        public IPAddress GetDns() => dns;

        public string GetOutputPath() => outputFilePath;

        public List<string> GetDomains() => domains;

        public List<string> GetErrorMessages() => errorMessages;
    }
}
