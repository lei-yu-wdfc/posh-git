using System;

namespace Wonga.QA.Framework.Core
{
    public struct Date
    {
        public DateTime DateTime;
        public DateFormat DateFormat;

        public Date(DateTime date, DateFormat format = DateFormat.DateTime)
        {
            DateTime = date;
            DateFormat = format;
        }

        public static implicit operator DateTime(Date date)
        {
            return date.DateTime;
        }
    }

    public enum DateFormat
    {
        DateTime,
        Date,
        YearMonth
    }

    public static partial class Extensions
    {
        public static Date ToDate(this DateTime date, DateFormat format)
        {
            return new Date(date, format);
        }
    }
}
