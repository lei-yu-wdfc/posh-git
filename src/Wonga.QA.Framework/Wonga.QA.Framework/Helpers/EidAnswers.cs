using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wonga.QA.Framework.Helpers
{
    public static class EidAnswers
    {
		static readonly Dictionary<Regex, String> EidAnswersRegexPatterns = new Dictionary<Regex, String>
        {
            {new Regex("^Your credit file indicates you may have an auto loan/lease which was opened approximately [a-zA-Z 0-9]+. Please choose the credit provider for this account from the following options.$"), "ALLY CREDIT CANADA LIMITED"},
            {new Regex("^Your credit file indicates you may have a gas card or account which was opened approximately [a-zA-Z 0-9]+. Please choose the credit provider for this account from the following options.$"), "SHELL OIL"}
        };

        public static String GetEidAnswer(String question)
        {
			foreach (KeyValuePair<Regex, string> patternValuePair in EidAnswersRegexPatterns)
        	{
        		if(patternValuePair.Key.IsMatch(question))
        		{
        			return patternValuePair.Value;
        		}
        	}

			return null;
        }
    }
}