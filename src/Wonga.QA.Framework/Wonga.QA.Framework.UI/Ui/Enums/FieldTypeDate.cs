using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Ui.Enums
{
    public enum FieldTypeDate
    {
        LessThan = 1,
        MoreThan = 2,

        // UK formats
        //  dd Month yyyy
        UkFullNow = 3,
        UkFullRandom = 4,
        // dd Month yyyy
        UkLongNow = 5,
        UkLongRandom = 6,
        // dd-Mon-yyyy
        UkMediumNow = 7,
        UkMediumRandom = 8,
        // dd/mm/yy
        UkShortNow = 9,
        UkShortRandom = 10,

    }
}
