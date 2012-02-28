﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Wonga.QA.Framework.Core
{
    public static class Data
    {
        private static String _alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static Random _random = new Random(Guid.NewGuid().GetHashCode());

        public static Guid GetId()
        {
            return Guid.NewGuid();
        }

        public static DateTime GetDateTimeMin()
        {
            return SqlDateTime.MinValue.Value;
        }

        public static Date GetDoB()
        {
            return RandomDate(new DateTime(1900, 1, 1), DateTime.Today.AddYears(-18));
        }

        public static String GetEmail()
        {
            return String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
        }

        public static String GetEmailWithoutPlusChar()
        {
            return String.Format("qa.wonga.com{0}@gmail.com", Guid.NewGuid());
        }

        public static String GetEmployerName()
        {
            return "Test:EmployedMask";
        }

		public static Date GetNextPayDate()
		{
			return new Date(DateTime.UtcNow.AddDays(10), DateFormat.Date);
		}

        public static String GetMiddleName()
        {
            return Config.AUT == AUT.Wb ? "TESTNoCheck" : "MiddleName";
        }

        public static String GetPassword()
        {
            return "Passw0rd";
        }

        public static String GetName()
        {
            return RandomString(2, 30);
        }

        public static String GetPhone()
        {
            return "0210000000";
        }

        public static String GetNationalNumber()
        {
            return "000000000";
        }

        public static decimal GetLoanAmount()
		{
			switch (Config.AUT)
			{
					case AUT.Za:
					{
						return 500;

					}
					case AUT.Uk:
					{
						return 100;

					}
					case AUT.Ca:
					{
						return 100;
					}
					case AUT.Wb:
					{
						return 1000;
					}

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		public static Date GetPromiseDate()
		{
			return new Date(DateTime.UtcNow.AddDays(10), DateFormat.Date);
		}

        public static String GetNIN(DateTime dob, Boolean female)
        {
			switch (Config.AUT)
        	{
				case AUT.Za:
        			{
						Int32[] nin = String.Format("{0:yyMMdd}{1:D4}{2}{3}", dob, female ? _random.Next(5000) : _random.Next(5000, 10000), _random.Next(2), _random.Next(10)).Select(c => Int32.Parse(c.ToString())).ToArray();
						Int32 z = 10 - (((nin.Where((n, i) => i % 2 == 0).Sum()) + ((2 * (Int32.Parse(String.Join(null, nin.Where((n, i) => i % 2 == 1))))).ToString().Select(c => Int32.Parse(c.ToString())).Sum())) % 10);
						return String.Format("{0}{1}", String.Join(null, nin), z == 10 ? 0 : z);
        			}
				default:
        			{
        				throw new NotImplementedException(Config.AUT.ToString());
        			}
        	}
        }

        public static Int32 RandomInt(Int32 min, Int32 max)
        {
            return _random.Next(min, max);
        }

        public static Int32 RandomInt(Int32 max)
        {
            return _random.Next(max);
        }

		public static Int64 RandomLong(Int64 min, Int64 max)
		{
			if(min > max)
			{
				throw new ArgumentOutOfRangeException("min", "min is greater than max");
			}

			return min + Convert.ToInt64((max - min)*_random.NextDouble());
		}

        public static Double Random()
        {
            return _random.NextDouble();
        }

        public static Date RandomDate()
        {
            return RandomDate(new DateTime(1900, 1, 1), DateTime.Today);
        }

        public static DateTime RandomDateTime()
        {
            return RandomDateTime(new DateTime(1900, 1, 1), DateTime.Now);
        }

        public static Date RandomDate(DateTime min, DateTime max)
        {
            return min.AddDays(_random.Next((max - min).Days)).ToDate(DateFormat.Date);
        }

        public static DateTime RandomDateTime(DateTime min, DateTime max)
        {
            return min.AddTicks((Int64)((max.Ticks - min.Ticks) * _random.NextDouble()));
        }

        public static T RandomEnum<T>()
        {
            return RandomElement((T[])Enum.GetValues(typeof(T)));
        }

		public static T RandomEnum<T>(params T[] exclude)
		{
			return RandomElement(((T[])Enum.GetValues(typeof(T))).Where(t=>exclude == null || !exclude.Contains(t)).ToArray());
		}


        public static string EnumToString(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static String InvertCase(String text)
        {
            return new string(text.Select(c => char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c)).ToArray());
        }

        public static T RandomElement<T>(IList<T> enumerable)
        {
            return enumerable.ElementAt(_random.Next(enumerable.Count));
        }

        public static String RandomString(Int32 length)
        {
            return RandomString(length, length);
        }

        public static String RandomString(Int32 min, Int32 max)
        {
            StringBuilder builder = new StringBuilder();
            for (Int32 i = 0; i < _random.Next(min, max); i++)
                builder.Append(_alpha[_random.Next(0, _alpha.Length)]);
            return builder.ToString();
        }

        public static Boolean RandomBoolean()
        {
            return _random.NextDouble() < 0.5;
        }

        public static String Indent(String value)
        {
            try { return XDocument.Parse(value).ToString(); }
            catch { return value; }
        }

        public static String ToString(Object value)
        {
            if (value == null)
                return null;
            if (value is Boolean)
                return ((Boolean)value).ToString().ToLower();
            if (value is DateTime)
                return ((DateTime)value).ToString("s");
            if (value is Byte[])
                return Convert.ToBase64String((Byte[])value);

            return value.ToString();
        }
    }
}
