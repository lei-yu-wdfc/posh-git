using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public abstract class CsGetLoanExtensionStatusQueryTests
    {
        protected Application _application;
        protected Customer _customer;

        [SetUp]
        public virtual void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(100)
                                             .WithLoanTerm(7)
                                             .Build();
        }

        public abstract class GivenACustomerWithAnApprovedLoan : CsGetLoanExtensionStatusQueryTests
        {
            protected Guid _extensionId;

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                //Request an extension.
                //Drive.Cs.Commands.Post(new CsExtendLoan);

            }

            public class WhenAnExtensionIsRequested : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk)]
                public void TheStatusOfTheExensionCanBeObtained()
                {
                    CsResponse res = Drive.Cs.Queries.Post(new CsGetLoanExtensionStatusQuery { CsUser = "csUser", ExtensionId = _extensionId });

                    //Assert.IsTrue(res.Values["ExtensionId"].Count() == 1);
                    
                }
            }
        }

    }
}
