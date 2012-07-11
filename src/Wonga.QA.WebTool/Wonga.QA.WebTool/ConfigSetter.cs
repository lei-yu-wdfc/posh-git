using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.WebTool
{
    public class ConfigSetter
    {
        public static void Setter(string aut, string sut)
        {
            string targetDirectory = @"C:\Users\kirill.polishyk\AppData\Roaming\V3QA";
            string sourceDirectory = @"C:\Wonga.QA.WebTool\QAF\run\config";
            string file = "";

            switch (sut)
            {
                case "RC":
                    file = aut.ToLower() + "_" + "rc_master" + ".v3qaconfig";
                    break;
                case "WIPRelase":
                    file = aut.ToLower() + "_" + "wip_release" + ".v3qaconfig";
                    break;
                case "RCRelease":
                    file = aut.ToLower() + "_" + "rc_release" + ".v3qaconfig";
                    break;
                case "WIP":
                    file = aut.ToLower() + "_" + "wip_master" + ".v3qaconfig";
                    break;
            }
            if (Directory.Exists(targetDirectory)) System.IO.Directory.Delete(targetDirectory, true);
            System.IO.Directory.CreateDirectory(targetDirectory);

            string sourceFile = System.IO.Path.Combine(sourceDirectory, file);
            string destFile = System.IO.Path.Combine(targetDirectory, file);

            System.IO.File.Copy(sourceFile, destFile, true);
        }
    }
}