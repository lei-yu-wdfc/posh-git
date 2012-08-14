using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Runtime.FileTypes;
using Wonga.QA.Framework.UI.Ui.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Pages.Helpers
{
    public class ValidationHelper<TPage>
        where TPage : BasePage
    {
        private static TPage _page;
        private static String _fieldName;
        private static ExecutableFunction _baseCallBack;

        public delegate void ExecutableFunction(TPage page, String fieldName, String value);


        public static void CheckValidation(TPage page, String fieldName, List<Int32> restrictionList, FieldTypeList restrictionType, Delegate callBack, List<KeyValuePair<Int32, Delegate>> customRules, FieldType fieldType)
        {
            _page = page;
            _fieldName = fieldName;

            ExecutableFunction callBackFunction = new ExecutableFunction((ExecutableFunction)callBack);
            _baseCallBack = callBackFunction;

            List<Int32> list = EnumList(restrictionList, restrictionType, fieldType);
            ValidateByType(fieldType, list, customRules);
        }

        private static void CallBack(String value)
        {
            if (value != null)
            {
                ExecutableFunction executableFunction = new ExecutableFunction(_baseCallBack);
                executableFunction(_page, _fieldName, value);
            }
        }

        private static List<Int32> GetFullEnumList(FieldType type)
        {
            List<Int32> fullList = new List<Int32>();
            switch (type)
            {
                case FieldType.String:
                    fullList = Array.ConvertAll(Enum.GetValues(typeof(FieldTypeString)).Cast<FieldTypeString>().ToArray(), value => (Int32)value).ToList();
                    break;
            }
            return fullList;
        }
        private static void ValidateByType(FieldType fieldType, List<Int32> list, List<KeyValuePair<Int32, Delegate>> customCallBacks)
        {
            customCallBacks = customCallBacks ?? new List<KeyValuePair<Int32, Delegate>>();

            switch (fieldType)
            {
                case FieldType.String:
                    ValidationRulesHelper.ValidateForString(list, CallBack, customCallBacks);
                    break;
                case FieldType.Date:
                    break;
                case FieldType.DateTime:
                    break;
            }
        }
        private static List<Int32> EnumList(List<Int32> restrictionList, FieldTypeList restrictionType, FieldType fieldType)
        {
            List<Int32> fullList = GetFullEnumList(fieldType);
            switch (restrictionType)
            {
                case FieldTypeList.IncludeArray:
                    fullList = fullList.Where(item => restrictionList.Contains(item)).ToList();
                    break;
                case FieldTypeList.ExcludeArray:
                    fullList = fullList.Where(item => !restrictionList.Contains(item)).ToList();
                    break;
            }
            return fullList;
        }

    }
}
