using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.Self)]
    public class GetValidTransactionTypesQueryTest
    {
        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void ShouldReturnOnlyExpectedTransactionTypes()
        {
            var response= Drive.Cs.Queries.Post(new Wonga.QA.Framework.Cs.GetValidTransactionTypesQuery());

            Assert.AreEqual(9, response.Values["Type"].Count());
            Assert.Contains(response.Values["Type"], "CardPayment");
            Assert.Contains(response.Values["Type"], "CashAdvance");
            Assert.Contains(response.Values["Type"], "Cheque");
            Assert.Contains(response.Values["Type"], "DefaultCharge");
            Assert.Contains(response.Values["Type"], "DirectBankPayment");
            Assert.Contains(response.Values["Type"], "Fee");
            Assert.Contains(response.Values["Type"], "InterestAdjustment");
            Assert.Contains(response.Values["Type"], "Interest");
            Assert.Contains(response.Values["Type"], "WriteOff");
        }
    }
}
