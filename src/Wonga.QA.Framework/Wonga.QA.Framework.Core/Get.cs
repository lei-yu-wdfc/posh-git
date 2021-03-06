﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Core
{
    public static class Get
    {
        private static String _alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static String _alphaNumeric = _alpha + "0123456789";
        private static Random _random = new Random(Guid.NewGuid().GetHashCode());
        private static readonly string EmailSafeMachineName;

        static Get()
        {
            EmailSafeMachineName = Regex.Replace(Environment.MachineName, @"[^a-zA-Z0-9-._]", @"-");
        }

        public static Guid GetId()
        {
            return Guid.NewGuid();
        }

        public static String GetId(Guid id)
        {
            return (id.ToString().Replace("-", string.Empty));
        }

        public static T EnvironmentVariable<T>(object defaultValue = null, string variable = null)
        {
            Object value = Registry.CurrentUser.OpenSubKey("Environment").GetValue(variable ?? typeof(T).Name) ??
                defaultValue ??
                default(T);

            return (T)(typeof(T).IsEnum ? Enum.Parse(typeof(T), value.ToString(), true) : Convert.ChangeType(value, typeof(T)));
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
            return "qa.wonga.com@gmail.com";
        }

        public static T EnumFromString<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

        public static String GetEmail(int mailLength)
        {
            String guid = Guid.NewGuid().ToString();
            return String.Format("qa.wonga.com{0}@gmail.com", guid.Substring(0, mailLength - GetEmail().Length - 1));
        }

        public static String RandomEmail()
        {
            return String.Format(
                "qa.wonga.com+{0}-{1}@gmail.com",
                EmailSafeMachineName,
                Guid.NewGuid());
        }

        public static String GetEmailWithoutPlusChar()
        {
            return String.Format("qa.wonga.com{0}{1}@gmail.com", EmailSafeMachineName, Guid.NewGuid());
        }

        public static String GetEmployerName()
        {
            return "TESTEmployedMask";
        }

        public static String GetEmploymentStatus()
        {
            return "EmployedFullTime";
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
            return RandomString(4, 30);
        }

        public static String GetPhone()
        {

            string prefix = null;
            switch (Config.AUT)
            {
                case AUT.Uk:
                case AUT.Wb:
                    prefix = "0287";
                    break;
                default:
                    prefix = "021";
                    break;
            }
            return prefix + _random.Next(1000000, 9999999).ToString(CultureInfo.InvariantCulture);
        }

        public static string GetMobilePhone()
        {
            switch (Config.AUT)
            {
                case AUT.Ca:
                    {
                        return "9876543219";
                    }
                case AUT.Za:
                    {
                        return "021" + RandomLong(1000000, 9999999);
                    }
                case AUT.Wb:
                    {
                        return "07700900" + RandomLong(100, 999);
                    }
                case AUT.Pl:
                    {
                        return "2799990000" + RandomLong(1, 9);
                    }
                default:
                    {
                        return "07700900" + RandomLong(100, 999);
                    }
            }
        }

        public static string GetVerificationPin()
        {
            return "0000";
        }

        public static String GetNationalNumber()
        {
            return "000000000";
        }

        public static String GetNationalNumber(DateTime dob, Boolean female)
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
                        return 10000;
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

        public static DateTime GetApplicationDate()
        {
            return DateTime.Now;
        }

        public static string GetBankAccountNumber(string bankSortCode = "938600")
        {
            switch (Config.AUT)
            {
                case AUT.Ca:
                    {
                        return RandomLong(1000000, 9999999).ToString();
                    }
                case AUT.Za:
                    {
                        return RandomLong(10000000000, 99999999999).ToString();
                    }
                case AUT.Uk:
                    {
                        return RandomUkBankAccountFromSortCode(bankSortCode);
                    }
                case AUT.Wb:
                    {
                        //return RandomLong(10000000, 99999999).ToString();
                        return RandomUkBankAccountFromSortCode(bankSortCode);
                    }
                default:
                    {
                        throw new NotImplementedException(Config.AUT.ToString());
                    }
            }
        }

        private static string RandomUkBankAccountFromSortCode(string bankSortCode)
        {
            //backward compatibility
            switch (bankSortCode)
            {
                case "938600":
                    return "42368003";

                case "161027":
                    return "10032650";

                case "180002":
                    return "00000190";

                case "070116":
                    return "34012583";

                case "074456":
                    return "12345112";

                case "204422":
                    {
                        //tried to implement DoubleAlternateModulus10 but it was failing in UnifiedSoftwareBankValidation
                        var validAccounts = new[]
						                    	{
						                    		"33069079",
						                    		"42574721",
						                    		"44555090",
						                    		"54827422",
						                    		"56320888"
						                    	};
                        return validAccounts[RandomInt(0, validAccounts.Length)];
                    }
            }

            throw new NotImplementedException(string.Format("UK Account Generator not implemented for sort code: {0}", bankSortCode));
        }

        public static Guid GetCsAuthorization()
        {
            return Guid.Parse("93370527-1BEE-461B-B825-07A6BE7AB8FE");
        }

        public static String GetPostcode()
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                case AUT.Pl:
                case AUT.Wb:
                    {
                        return "SW6 6PN";
                    }
                case AUT.Ca:
                    {
                        return "V4F3A9";
                    }
                default:
                    {
                        return RandomInt(0000, 9999).ToString().PadLeft(4, '0');
                    }
            }

        }

        public static String GetCountryCode()
        {
            String result = "";
            switch (Config.AUT)
            {
                case AUT.Uk:
                    {
                        result = "UK";
                    }
                    break;
            }

            return result;
        }

        public static Uri GetSchema(Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Path += "/Api.xsd";
            return builder.Uri;
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
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min", "min is greater than max");
            }

            return min + Convert.ToInt64((max - min) * _random.NextDouble());
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
            return RandomElement(((T[])Enum.GetValues(typeof(T))).Where(t => exclude == null || !exclude.Contains(t)).ToArray());
        }

        public static T RandomEnumOf<T>(params T[] allowed)
        {
            if (allowed == null)
            {
                throw new ArgumentNullException("allowed");
            }

            if (allowed.Length == 0)
            {
                throw new ArgumentOutOfRangeException("allowed", "collection of enum values can not be empty");
            }

            return RandomElement(((T[])Enum.GetValues(typeof(T))).Where(allowed.Contains).ToArray());
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

        public static String RandomAlphaNumeric(Int32 min, Int32 max)
        {
            StringBuilder builder = new StringBuilder();
            for (Int32 i = 0; i < _random.Next(min, max); i++)
                builder.Append(_alphaNumeric[_random.Next(0, _alphaNumeric.Length)]);
            return builder.ToString();
        }

        public static Boolean RandomBoolean()
        {
            return _random.NextDouble() < 0.5;
        }

        public static String Indent(String value)
        {
            try { return XElement.Parse(value).ToString(); }
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

            //todo
            if (value is Array)
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, value);
                XElement element = XElement.Parse(writer.ToString());
                StringBuilder builder = new StringBuilder();
                element.Elements().ToList().ForEach(e => builder.AppendLine(e.ToString()));
                return builder.ToString();
            }

            return value.ToString();
        }

        #region DataValidation

        public static String SpecialSymbolsRandomString(Int32 size)
        {
            Random _rng = new Random();
            String _chars = "!@#$%^&*()_<>";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public static String toSelect()
        {
            return "-- Please Select --";
        }

        #endregion
    }
}
