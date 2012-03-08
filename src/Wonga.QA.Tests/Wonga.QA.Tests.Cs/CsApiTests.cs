using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Cs
{
    [Parallelizable(TestScope.All)]
    public class CsApiTests
    {
        [Test]
        public void HelloWorld()
        {
            ValidatorException exception = Assert.Throws<ValidatorException>(() => Driver.Cs.Queries.Post(new GetRepaymentArrangementsQuery { ApplicationId = Guid.NewGuid() }));
            Assert.Contains(exception.Errors, "Payments_Application_NotFound");
        }
    }
}
