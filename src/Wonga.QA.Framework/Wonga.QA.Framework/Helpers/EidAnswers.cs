using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Helpers
{
    public static class EidAnswers
    {
        static readonly Dictionary<String, String> eidAnswers = new Dictionary<String, String>
        {
            {"Your credit file indicates you may have an auto loan/lease which was opened approximately January 2003. Please choose the credit provider for this account from the following options.", "ALLY CREDIT CANADA LIMITED"},
            {"Your credit file indicates you may have a gas card or account which was opened approximately August 2004. Please choose the credit provider for this account from the following options.", "SHELL OIL"}
        };

        public static String GetEidAnswer(String question)
        {
            String result;
            return eidAnswers.TryGetValue(question, out result) ? result : null;
        }
    }
}