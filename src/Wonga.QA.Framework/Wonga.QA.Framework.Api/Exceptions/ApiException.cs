using System;
using System.Collections.Generic;

namespace Wonga.QA.Framework.Api.Exceptions
{
    public class ApiException : Exception
    {
        public IEnumerable<String> Errors { get; set; }

        public ApiException(IEnumerable<String> errors, Exception inner = null)
            : base(String.Join(",", errors), inner)
        {
            Errors = errors;
        }
    }
}
