using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Wonga.QA.Generators.Core
{
    public static class Settings
    {
        const string V3_ORIGIN = "V3Origin";
        public static void SetOrigin(string value)
        {
            SetSetting(V3_ORIGIN, value);
        }

        public static string GetOrigin()
        {
            return GetSetting(V3_ORIGIN);
        }

        private static void SetSetting(string name, object value)
        {
            Registry.CurrentUser.OpenSubKey("Environment").SetValue(V3_ORIGIN, value);
        }

        private static string GetSetting(string name)
        {
            var envVar = Registry.CurrentUser.OpenSubKey("Environment").GetValue(name);
            return envVar != null ? envVar.ToString() : null;
        }
    }
}
