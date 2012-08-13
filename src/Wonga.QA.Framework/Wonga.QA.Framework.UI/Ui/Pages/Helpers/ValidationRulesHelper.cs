using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Ui.Enums;

namespace Wonga.QA.Framework.UI.Ui.Pages.Helpers
{
    public class ValidationRulesHelper
    {
        public delegate void Callback(String value);
        public delegate String CustomCallback();

        public static void ValidateForString(List<Int32> list, Callback callback, Dictionary<Int32, Delegate> customCallBacks)
        {
            foreach (Int32 item in list)
            {
                String value = null;
                if (customCallBacks.Count() != 0 && customCallBacks[item] != null)
                {
                    CustomCallback customFunction = new CustomCallback((CustomCallback)customCallBacks[item]);
                    value = customFunction();
                }
                else
                    switch ((FieldTypeString)item)
                    {
                        case FieldTypeString.Letters:
                            value = GetStringLetters();
                            break;
                        case FieldTypeString.NegativeNumbers:
                            value = GetStringNegativeNumbers();
                            break;
                        case FieldTypeString.Numbers:
                            break;
                        case FieldTypeString.NumbersWithComa:
                            value = GetStringNumbersWithComa();
                            break;
                        case FieldTypeString.SpecialSymbols:
                            value = GetRandomSpecialSymbolsString(10);
                            break;
                    }

                Callback function = new Callback(callback);
                function(value);
            }
        }

        private static String GetStringLetters()
        {
            return Get.GetName();
        }

        private static String GetStringNegativeNumbers()
        {
            return Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00").Insert(0, "-");
        }

        private static String GetStringNumbersWithComa()
        {
            return Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00").Replace('.', ',');
        }

        private static String GetRandomSpecialSymbolsString(Int32 size)
        {
            Random _rng = new Random();
            String _chars = "!@#$%^&*()_<>";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
    }
}
