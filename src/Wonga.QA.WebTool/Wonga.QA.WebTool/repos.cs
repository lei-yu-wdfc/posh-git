using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using GitSharp;

namespace Wonga.QA.WebTool
{
    static public class repos
    {
        static public string Update()
        {
            try
            {
                ProcessStartInfo gitInfo = new ProcessStartInfo();
                gitInfo.CreateNoWindow = true;
                gitInfo.RedirectStandardError = true;
                gitInfo.RedirectStandardOutput = true;
                gitInfo.UseShellExecute = false;
                gitInfo.FileName = @"C:\Program Files (x86)\Git\bin\git.exe";


                Process gitProcess = new Process();
                gitInfo.Arguments = "pull";
                gitInfo.WorkingDirectory = @"C:\Users\kirill.polishyk\Desktop\Git\v3QA";

                gitProcess.StartInfo = gitInfo;
                gitProcess.Start();

                string stderr_str = gitProcess.StandardError.ReadToEnd(); // pick up STDERR
                string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

                gitProcess.WaitForExit();
                gitProcess.Close();
                return stdout_str;
            }
            catch(Exception exc)
            {
                return exc.Message;
            }
        }
    }
}