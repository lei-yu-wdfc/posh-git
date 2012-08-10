using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Ui.Enums;

namespace Wonga.QA.Framework.UI.Ui.Pages.Helpers
{
    public class DataValidation
    {
        public String FieldName { get; private set; }
        public String FieldValue { get; private set; }
        public FieldType FieldType { get; private set; }
        public FieldTypeList FieldTypeList { get; private set; }
        public List<Int32> RulesArray { get; private set; }
        public Boolean IsExtended { get; private set; }

        public DataValidation(String fieldName, String fieldValue, FieldType fieldType)
            : this(fieldName, fieldValue, fieldType, FieldTypeList.All, null)
        {
            IsExtended = false;

            FieldName = fieldName;
            FieldValue = fieldValue;
            FieldType = fieldType;
        }
        public DataValidation(String fieldName, String fieldValue, FieldType fieldType, FieldTypeList fieldTypeList, List<Int32> rulesArray)
        {
            IsExtended = true;

            FieldName = fieldName;
            FieldValue = fieldValue;
            FieldType = fieldType;
            FieldTypeList = fieldTypeList;
            RulesArray = rulesArray;
        }
    }
}
