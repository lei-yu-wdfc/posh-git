using System;
using System.Collections.Generic;

namespace Wonga.QA.Framework.Api.Exceptions
{
    public class XsdException : ApiException
    {
        public XsdException(IEnumerable<String> errors, Exception inner)
            : base(errors, inner)
        {
        }
    }
}
