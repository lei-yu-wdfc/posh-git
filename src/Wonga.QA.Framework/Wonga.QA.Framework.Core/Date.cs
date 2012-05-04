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

        public override String ToString()
        {
            switch (DateFormat)
            {
                case DateFormat.DateTime:
                    return DateTime.ToString("s");
                case DateFormat.YearMonth:
                    return DateTime.ToString("yyyy-MM");
                case DateFormat.Date:
                    return DateTime.ToString("yyyy-MM-dd");
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an ordinal date (with "st", "nd", "rd", or "th") as a string.
        /// </summary>
        /// <param name="date">A date of DateTime type</param>
        /// <param name="format">The needed format of the date</param>
        /// <returns>String representing the date in the ordinal format</returns>
        public static string GetOrdinalDate(DateTime date, string format)
        {
            var cDate = date.Day.ToString("d")[date.Day.ToString("d").Length - 1];
            string suffix;
            if ((date.Day.ToString("d").Length == 2) && (date.Day.ToString("d")[0] == '1'))
                suffix = "th";
            else
                switch (cDate)
                {
                    case '1':
                        suffix = "st";
                        break;
                    case '2':
                        suffix = "nd";
                        break;
                    case '3':
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            var sDate = date.Day.ToString("d") + " ";
            var sDateOrdinial = date.Day.ToString("d") + suffix + " ";
            return date.ToString(format).Replace(sDate, sDateOrdinial);
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
        public static Date ToDate(this DateTime date, DateFormat format = DateFormat.DateTime)
        {
            return new Date(date, format);
        }
    }
}
