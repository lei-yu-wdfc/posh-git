using System;
using System.Collections.Generic;

namespace Wonga.QA.Framework.Api.Exceptions
{
    public class SchemaException : ValidatorException
    {
        public SchemaException(IEnumerable<String> errors, Exception inner)
            : base(errors, inner)
        {
        }
    }
}
