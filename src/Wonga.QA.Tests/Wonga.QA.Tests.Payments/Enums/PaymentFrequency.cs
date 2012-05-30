using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.Payments.Enums
{
    public enum PaymentFrequency
    {
        Monthly = 1,
        TwiceMonthly = 2,
        Weekly = 3,
        LastDayOfMonth = 4,
        LastFridayOfMonth = 5,
        BiWeekly = 6
    }
}
