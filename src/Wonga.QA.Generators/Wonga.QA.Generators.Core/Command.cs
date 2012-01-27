using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Wonga.QA.Generators.Core
{
    public static class Command
    {
        private static String[] _error = new[] { "fatal", "error", "exception" };
        private static String[] _warning = new[] { "warn" };

        public static void Run(String command, Int32 logo, IEnumerable<String> arguments, params Object[] parameters)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = String.Format(String.Join(" ", arguments), parameters),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.OutputDataReceived += (s, a) => Output(a.Data, ref logo);
            process.ErrorDataReceived += (s, a) => Error(a.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception(process.ExitCode.ToString());
        }

        private static void Output(String data, ref Int32 logo)
        {
            if (String.IsNullOrEmpty(data) || logo-- > 0)
                return;
            Console.ForegroundColor = _error.Any(data.ToLower().Contains) ? ConsoleColor.Red : _warning.Any(data.ToLower().Contains) ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        private static void Error(String data)
        {
            if (String.IsNullOrEmpty(data))
                return;
            throw new Exception(data);
        }
    }
}
