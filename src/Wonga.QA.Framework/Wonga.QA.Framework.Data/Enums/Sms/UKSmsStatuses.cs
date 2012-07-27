using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Data.Enums.Sms
{
    public enum UkSmsStatuses
    {
        New = 0,
        Acked = 1,
        Delivered = 2,
        Failed = 3,
        BufferedPhone = 4,
        BufferedSmsc = 5,
        NonDelivered = 6,
        LostNotification = 7,
        UnknownNotification = 8
    }
}