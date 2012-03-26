using System;

namespace Wonga.QA.Framework.Helpers
{
    public static class StringExtensions
    {
        public static string MaskedCardNumber(this String cardNumber)
        {
            if (String.IsNullOrEmpty(cardNumber))
                return null;

            return "**** **** **** " + cardNumber.Substring(cardNumber.Length - 4);
        }
    }
}