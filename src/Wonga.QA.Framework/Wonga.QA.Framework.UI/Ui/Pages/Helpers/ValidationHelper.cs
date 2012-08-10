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
    public class ValidationHelper<T, C>
        where T : struct
        where C : BasePage
    {
        static ValidationHelper()
        {
            if (!IsEnum())
                throw new Exception("Type should be System.Enum type");
        }

        private static C _page;
        private static String _fieldName;

        public delegate void ExecutableFunction(C page, String fieldName, String value);

        public static void CheckValidation(C page, String fieldName, List<T> restrictionList, FieldTypeList restrictionType, ExecutableFunction function)
        {
            _page = page;
            _fieldName = fieldName;

            List<T> list = EnumList(restrictionList, restrictionType);
            ValidateByType(FieldType.String, list, function);
        }

        private static Boolean IsEnum()
        {
            return typeof(T).IsEnum;
        }

        private static List<T> EnumToList()
        {
            Type enumType = typeof(T);
            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }
            return enumValList;
        }

        private static void ValidateByType(FieldType fieldType, List<T> list, ExecutableFunction function)
        {
            switch (fieldType)
            {
                case FieldType.String:
                    ValidateForString(list, function);
                    break;
                case FieldType.Date:
                    break;
                case FieldType.DateTime:
                    break;
            }
        }
        private static List<T> EnumList(List<T> restrictionList, FieldTypeList restrictionType)
        {
            List<T> fullList = EnumToList();
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

        // custom validation
        private static void ValidateForString(List<T> list, ExecutableFunction function)
        {
            foreach (T item in list)
            {
                FieldTypeString currentType = (FieldTypeString)Enum.Parse(typeof(FieldTypeString), item.ToString());
                String value = String.Empty;

                switch (currentType)
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
                        value = GetRandomSpecialSymbolsString();
                        break;
                    default:
                        return;
                }

                ExecutableFunction executableFunction = new ExecutableFunction(function);
                executableFunction(_page, _fieldName, value);
            }
        }

        private static String GetRandomSpecialSymbolsString()
        {
            int lenght = Get.RandomInt(0, 10);
            String strForOut = "";
            List<char> specialsymbols = new List<char>();
            specialsymbols.AddRange(@"!@#$%^&*()_".ToCharArray());
            for (int i = 0; i < lenght; i++)
            {
                strForOut += Get.RandomElement<char>(specialsymbols).ToString();
            }
            return strForOut;
        }

    }
}
