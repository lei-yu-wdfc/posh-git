using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.CallReport
{
    [Parallelizable(TestScope.All)]
    class CustomerIsAliveTests
    {
        private String _maskName;
        private String _checkPointName;
        private String _expectedVerification;

        [SetUp, AUT]
        public void Setup()
        {
            _maskName = "ApplicantIsNotDeceased";
            _checkPointName = "ApplicantIsNotDeceased";
            _expectedVerification = "CreditBureauCustomerIsAliveVerification";
        }

        [Test, JIRA("SME-XXX"),AUT(AUT.Wb)]
        public void TestUnknownApplicant_LoanIsApproved()
        {
            var customer = CustomerBuilder.New().WithMiddleName("TEST"+_maskName).Build();
            var organization = OrganisationBuilder.New(Data.GetId()).WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
        }
    }
}
