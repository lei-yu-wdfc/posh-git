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

        private static CustomCallback _customCallBack;

        public static void ValidateForString(List<Int32> list, Callback callback, List<KeyValuePair<Int32, Delegate>> customCallBacks)
        {
            foreach (Int32 item in list)
            {
                String value = null;
                if (customCallBacks.Count() != 0 && customCallBacks.Where(z => z.Key == item).Select(p => new { Key = p.Key, Value = p.Value }).FirstOrDefault() != null)
                {
                    foreach (KeyValuePair<Int32, Delegate> customCallBack in customCallBacks.Where(z => z.Key == item))
                    {
                        _customCallBack = (CustomCallback)Delegate.CreateDelegate(typeof(CustomCallback),
                                                            customCallBack.Value.Target,
                                                            customCallBack.Value.Method);
                        value = _customCallBack();
                        CallBack(callback, value);
                    }
                    break;
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

                CallBack(callback, value);
            }
        }

        public static void ValidateForSelect(List<Int32> list, Callback callback, List<KeyValuePair<Int32, Delegate>> customCallBacks)
        {
            foreach (Int32 item in list)
            {
                String value = null;
                if (customCallBacks.Count() != 0 && customCallBacks.Where(z => z.Key == item).Select(p => new { Key = p.Key, Value = p.Value }).FirstOrDefault() != null)
                {
                    foreach (KeyValuePair<Int32, Delegate> customCallBack in customCallBacks.Where(z => z.Key == item))
                    {
                        CustomCallback customFunction = (CustomCallback)Delegate.CreateDelegate(typeof(CustomCallback),
                                                           customCallBack.Value.Target,
                                                           customCallBack.Value.Method);
                        value = customFunction();
                        CallBack(callback, value);
                    }
                    break;
                }
                else
                    switch ((FieldTypeSelect)item)
                    {
                        case FieldTypeSelect.Equal:
                            value = GetSelectEqual();
                            break;
                    }

                CallBack(callback, value);
            }
        }

        public static void ValidateForDate(List<Int32> list, Callback callback, List<KeyValuePair<Int32, Delegate>> customCallBacks)
        {
            foreach (Int32 item in list)
            {
                String value = null;
                if (customCallBacks.Count() != 0 && customCallBacks.Where(z => z.Key == item).Select(p => new { Key = p.Key, Value = p.Value }).FirstOrDefault() != null)
                {
                    foreach (KeyValuePair<Int32, Delegate> customCallBack in customCallBacks.Where(z => z.Key == item))
                    {
                        CustomCallback customFunction = (CustomCallback)Delegate.CreateDelegate(typeof(CustomCallback),
                                                           customCallBack.Value.Target,
                                                           customCallBack.Value.Method);
                        value = customFunction();
                        CallBack(callback, value);
                    }
                    break;
                }
                else
                    switch ((FieldTypeDate)item)
                    {
                        case FieldTypeDate.Equal:
                            value = GetCurrentDateInShortFormat();
                            break;
                        case FieldTypeDate.Past:
                            value = GetPastDateInShortFormat();
                            break;
                        case FieldTypeDate.Future:
                            value = GetFutureDateInShortFormat();
                            break;
                    }

                CallBack(callback, value);
            }
        }

        private static void CallBack(Callback callback, String value)
        {
            Callback function = new Callback(callback);
            function(value);
        }

        #region StringFunctions
        private static String GetSelectRandomValue()
        {
            return Get.GetName();
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
        #endregion

        #region SelectFunctions
        private static String GetSelectEqual()
        {
            return "-- Please Select --";
        }
        #endregion

        #region DateFunctions
        private static String GetCurrentDateInShortFormat()
        {
            return DateTime.UtcNow.ToShortDate();
        }

        private static String GetPastDateInShortFormat()
        {
            return DateTime.UtcNow.AddDays(-Get.RandomInt(1, 30)).ToShortDate();
        }

        private static String GetFutureDateInShortFormat()
        {
            return DateTime.UtcNow.AddDays(Get.RandomInt(1, 30)).ToShortDate();
        }
        #endregion
    }
}
