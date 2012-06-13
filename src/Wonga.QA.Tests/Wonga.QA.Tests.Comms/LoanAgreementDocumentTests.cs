using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture, AUT(AUT.Ca), Parallelizable(TestScope.All)]
    public class LoanAgreementDocumentTests
    {
        protected const string EmailTemplateName = "Email.SendCollatedLegalDocumentsTemplate";

        public class GivenACustomerFromAlberta : LoanAgreementDocumentTests
        {
            private Customer _customer;

            [SetUp]
            public virtual void SetUp()
            {
                _customer = CustomerBuilder.New().ForProvince(ProvinceEnum.AB).Build();
            }

            public class WhenTheySignALoanApplication : GivenACustomerFromAlberta
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
                }

                [Test]
                public void ThenTheCustomerIsEmailedTheLoanAgreementsDocuments()
                {
                    AssertDocumentsEmailed(_customer);
                }
            }
        }

        public class GivenACustomerFromBritishColumbia : LoanAgreementDocumentTests
        {
            private Customer _customer;

            [SetUp]
            public virtual void SetUp()
            {
                _customer = CustomerBuilder.New().ForProvince(ProvinceEnum.BC).Build();
            }

            public class WhenTheySignALoanApplication : GivenACustomerFromBritishColumbia
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
                }

                [Test]
                public void ThenTheCustomerIsEmailedTheLoanAgreementsDocuments()
                {
                    AssertDocumentsEmailed(_customer);
                }
            }
        }

        public class GivenACustomerFromOntario : LoanAgreementDocumentTests
        {
            private Customer _customer;

            [SetUp]
            public virtual void SetUp()
            {
                _customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            }

            public class WhenTheySignALoanApplication : GivenACustomerFromOntario
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
                }

                [Test]
                public void ThenTheCustomerIsEmailedTheLoanAgreementsDocuments()
                {
                    AssertDocumentsEmailed(_customer);
                }
            }
        }
        
       
        private void AssertDocumentsEmailed(Customer cust)
        {
            List<EmailToken> emailTokens = GetEmailTokens(EmailTemplateName, cust.Email);

            AssertEmailContainsCostomerForname(emailTokens, cust.GetCustomerForename());
            AssertEmailContainsLoanAgreement(emailTokens);
            AssertEmailContainsPreAuthoriseDebit(emailTokens);
            AssertEmailContainsLoanAgreementCancellationNotice(emailTokens);
            AssertEmailContainsConfirmationOfPad(emailTokens);
        }

        private List<EmailToken> GetEmailTokens(String emailTemplateName, string email)
        {
            var db = new DbDriver().QaData;
            var emailId = Do.Until(() => db.Emails.Single(e => e.EmailAddress == email && e.TemplateName == GetEmailTemplateId(emailTemplateName)).EmailId);
            return db.EmailTokens.Where(et => et.EmailId == emailId).ToList();
        }

        private string GetEmailTemplateId(string emailTemplateName)
        {
            return Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
        }

        private void AssertEmailContainsCostomerForname(List<EmailToken> emailTokens, string forname)
        {
            const string tokenKey = "First_name";

            Assert.IsTrue(emailTokens.Any(t => t.Key == tokenKey));

            var token = emailTokens.Single(t => t.Key == tokenKey);

            Assert.AreEqual(token.Value, forname);
        }

        private void AssertEmailContainsLoanAgreement(List<EmailToken> emailTokens)
        {
            Assert.IsTrue(emailTokens.Any(t => t.Key == "Loan_Agreement"));
        }

        private void AssertEmailContainsPreAuthoriseDebit(List<EmailToken> emailTokens)
        {
            Assert.IsTrue(emailTokens.Any(t => t.Key == "Pre_Authorise_Debit"));
        }

        private void AssertEmailContainsLoanAgreementCancellationNotice(List<EmailToken> emailTokens)
        {
            Assert.IsTrue(emailTokens.Any(t => t.Key == "Loan_Agreement_Cancellation_Notice"));
        }

        private void AssertEmailContainsConfirmationOfPad(List<EmailToken> emailTokens)
        {
            Assert.IsTrue(emailTokens.Any(t => t.Key == "Confirmation_Of_Pad"));
        }

    }
}