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
                            value = Get.GetName();
                            break;
                        case FieldTypeString.NegativeNumbers:
                            value = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00").Insert(0, "-");
                            break;
                        case FieldTypeString.Numbers:
                            break;
                        case FieldTypeString.NumbersWithComa:
                            value = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00").Replace('.', ',');
                            break;
                        case FieldTypeString.SpecialSymbols:
                            value = Get.SpecialSymbolsRandomString(10);
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
                            value = Get.toSelect();
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
                }
                else
                {
                    switch ((FieldTypeDate)item)
                    {
                        case FieldTypeDate.Equal:
                            value = DateTime.UtcNow.ToShortDate();
                            break;
                        case FieldTypeDate.Past:
                            value = DateTime.UtcNow.AddDays(-Get.RandomInt(1, 30)).ToShortDate();
                            break;
                        case FieldTypeDate.Future:
                            value = DateTime.UtcNow.AddDays(Get.RandomInt(1, 30)).ToShortDate();
                            break;
                    }

                    CallBack(callback, value);
                }
            }
        }

        private static void CallBack(Callback callback, String value)
        {
            Callback function = new Callback(callback);
            function(value);
        }
    }
}
