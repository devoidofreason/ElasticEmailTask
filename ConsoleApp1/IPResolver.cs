using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Resolver
{
    internal class IPResolver
    {
        private const string nslookupCommandPattern = "nslookup -type=mx {0} {1}";
        private const string cmdName = "cmd.exe";
        private const string whiteSpace = " ";
        private const string defaultIp = "0.0.0.0";
        private const bool cmdRedirectStandardInput = true;
        private const bool cmdRedirectStandardOutput = true;
        private const bool cmdRedirectStandardError = true;
        private const bool cmdUseShellExecute = false;
        private Object reportCollectionLock = new Object();

        private List<string> domains;
        private List<string> report = new List<string>();
        private IPAddress dns;

        public IPResolver(List<string> domains, IPAddress dns)
        {
            this.domains = domains;
            this.dns = dns;
        }

        public List<string> GetReport()
        {
            Parallel.ForEach(domains, domain =>
            {
                AddToReport(BuildReportForDomain(domain));
            });

            return report;
        }

        private void AddToReport(List<string> arg)
        {
            lock (reportCollectionLock)
            {
                report.AddRange(arg);
            }
        }

        private string GetNslookupCommand(string domain) => string.Format(nslookupCommandPattern, domain, (dns == default) ? string.Empty : dns.ToString());

        private List<string> GetNslookupResult(string domain)
        {
            var cmd = new Process();
            cmd.StartInfo.FileName = cmdName;
            cmd.StartInfo.RedirectStandardInput = cmdRedirectStandardInput;
            cmd.StartInfo.RedirectStandardOutput = cmdRedirectStandardOutput;
            cmd.StartInfo.RedirectStandardError = cmdRedirectStandardError;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.UseShellExecute = cmdUseShellExecute;
            cmd.Start();
            cmd.StandardInput.WriteLine(GetNslookupCommand(domain));
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            var lines = cmd.StandardOutput.ReadToEnd()
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            lines.RemoveAll(line => !line.StartsWith(domain));

            return lines;
        }

        private IPAddress ResolveIpFromDomain(string domain)
        {
            var ret = IPAddress.Parse(defaultIp);
            try
            {
                var entry = Dns.GetHostEntry(domain);
                ret = entry.AddressList.FirstOrDefault();
            }
            catch (Exception) { }

            return ret;
        }

        private List<string> BuildReportForDomain(string domain)
        {
            var ret = GetNslookupResult(domain).Select(line => string.Concat(line + whiteSpace, ResolveIpFromDomain(line.Split(whiteSpace).Last()))).ToList();

            return ret;
        }
    }
}
