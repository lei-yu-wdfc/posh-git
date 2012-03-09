using System;

namespace Wonga.QA.Tests.Payments.Helpers.Ca
{
    public static class ExtensionMethods
    {
        public static int AssertLenght(this int number, int lenght)
        {
            if (number.ToString().Length > lenght)
            {
                throw new Exception(string.Format("Integer must be no longer than {0} digits", lenght));
            }

            return number;
        }

        public static long AssertLenght(this long number, int lenght)
        {
            if (number.ToString().Length > lenght)
            {
                throw new Exception(string.Format("Long must be no longer than {0} digits", lenght));
            }

            return number;
        }

        public static string AssertLenght(this string word, int lenght)
        {
            if (word.Length > lenght)
            {
                throw new Exception(string.Format("String must be no longer than {0} characters", lenght));
            }

            return word;
        }

        public static string ToStringWithPadLeft(this int number, int maxLenght)
        {
            return number.ToString().PadLeft(maxLenght, '0');
        }

        public static string ToStringWithPadLeft(this long number, int maxLenght)
        {
            return number.ToString().PadLeft(maxLenght, '0');
        }
    }
}