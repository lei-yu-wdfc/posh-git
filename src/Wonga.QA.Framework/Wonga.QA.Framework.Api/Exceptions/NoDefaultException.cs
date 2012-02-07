using System;

namespace Wonga.QA.Framework.Api.Exceptions
{
    public class NoDefaultException : NotImplementedException
    {
        public NoDefaultException(ApiRequest request)
            : base(String.Format("Override Default in {0}.", request.GetType().Name))
        {
        }
    }
}
