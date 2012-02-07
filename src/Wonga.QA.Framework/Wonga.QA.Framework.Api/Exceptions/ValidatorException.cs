using System;
using System.Collections.Generic;

namespace Wonga.QA.Framework.Api.Exceptions
{
    public class ValidatorException : Exception
    {
        public IEnumerable<String> Errors { get; set; }

        public ValidatorException(IEnumerable<String> errors, Exception inner = null)
            : base(String.Join(",", errors), inner)
        {
            Errors = errors;
        }
    }
}
