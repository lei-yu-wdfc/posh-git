using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Ui.Enums
{
    public enum FieldTypeDate
    {
        Equal = 1,
        Past = 2,
        Future = 3,

        // UK formats
        //  dd Month yyyy
        UkFullNow = 4,
        UkFullRandom = 5,
        // dd Month yyyy
        UkLongNow = 6,
        UkLongRandom = 7,
        // dd-Mon-yyyy
        UkMediumNow = 8,
        UkMediumRandom = 9,
        // dd/mm/yy
        UkShortNow = 10,
        UkShortRandom = 11,

    }
}
