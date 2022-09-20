using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Resolver
{
    internal class OutputManager
    {
        private const string reportHeader = "Provided dns: {0}\n";
        private const string defaultDnsText = "default";

        private string outputFilePath;
        private IPAddress dns;
        private List<string> report;

        public OutputManager(IPAddress dns, string outputFilePath, List<string> report)
        {
            this.dns = dns;
            this.outputFilePath = outputFilePath;
            this.report = report;
        }

        public void WriteReport()
        {
            var fullReport = new List<string>();
            fullReport.Add(string.Format(reportHeader, (dns == default) ? defaultDnsText : dns.ToString()));
            report.ForEach(line => fullReport.Add(line));
            if(outputFilePath == default)
            {
                fullReport.ForEach(line => Console.WriteLine(line));
            }
            else
            {
                using (StreamWriter file = new StreamWriter(outputFilePath))
                {
                    fullReport.ForEach(line => file.WriteLine(line));
                }
            }
        }
    }
}
