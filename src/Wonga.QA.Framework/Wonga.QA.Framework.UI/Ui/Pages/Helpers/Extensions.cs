using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Ui.Enums;

namespace Wonga.QA.Framework.UI.Ui.Pages.Helpers
{
    public static partial class Extensions
    {
        public static string ToShortDate(this DateTime value)
        {
            MonthShortName shortMonth = (MonthShortName)value.Month;
            return String.Format("{0} {1} {2}", value.Day, shortMonth.ToString(), value.Year);
        }
    }
}
