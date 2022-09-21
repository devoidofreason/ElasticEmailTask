using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Runtime.Loader;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Diagnostics;

namespace ElasticEmailTaskTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string testGmailArgs = "gmail.com";
        
        private const string resolverAppName = "Resolver ";
        private const string cmdName = "cmd.exe";
        private const bool cmdRedirectStandardInput = true;
        private const bool cmdRedirectStandardOutput = true;
        private const bool cmdRedirectStandardError = true;
        private const bool cmdUseShellExecute = false;

        private readonly string[] testGmailExpected = { "alt1.gmail-smtp-in.l.google.com",
                                                        "alt2.gmail-smtp-in.l.google.com",
                                                        "alt3.gmail-smtp-in.l.google.com",
                                                        "alt4.gmail-smtp-in.l.google.com",
                                                        "gmail-smtp-in.l.google.com" };

        private string RunCmd(string arg)
        {
            var cmd = new Process();
            cmd.StartInfo.FileName = cmdName;
            cmd.StartInfo.RedirectStandardInput = cmdRedirectStandardInput;
            cmd.StartInfo.RedirectStandardOutput = cmdRedirectStandardOutput;
            cmd.StartInfo.RedirectStandardError = cmdRedirectStandardError;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.UseShellExecute = cmdUseShellExecute;
            cmd.Start();
            cmd.StandardInput.WriteLine(arg);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            return cmd.StandardOutput.ReadToEnd();
        }

        [TestMethod]
        public void TestGmail()
        {
            var anyMissing = false;
            var testGmail = RunCmd(resolverAppName + testGmailArgs);
            foreach (var result in testGmailExpected)
            {
                int k = 0;
                if (!testGmail.Contains(result))
                {
                    anyMissing = true;
                }
            }
            Assert.AreEqual(anyMissing, false);
        }
    }
}
