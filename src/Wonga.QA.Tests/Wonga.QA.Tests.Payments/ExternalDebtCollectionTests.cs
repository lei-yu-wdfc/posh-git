using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class ExternalDebtCollectionTests
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void WhenApplicationMovedToDcaInterestAcrualShouldBeSuppressed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyInterestSuspended(application, DateTime.UtcNow.Date));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void WhenApplicationRevokedFromDcaInterestAcrualShouldBeRessumed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Drive.Cs.Commands.Post(new RevokeApplicationFromDcaCommand
                                        {
                                            ApplicationId = application.Id
                                        });

            Assert.IsTrue(VerifyPaymentFunctions.VerifyInterestResumed(application, DateTime.UtcNow.Date));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationHasNotBeenMovedToDca()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = application.Id
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_ApplicationHasNotBeenMovedToDca", exception.Errors.Single());
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationWasNotSpecified()
        {
            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = Guid.Empty
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_MissingApplicationId", exception.Errors.Single());
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationDoesNotExist()
        {
            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = Guid.NewGuid()
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_ApplicationDoesNotExist", exception.Errors.Single());
            }
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-1862")]
        //public void ShouldAddARevokeRecordToDebtCollections()
        //{
        //    var customer = CustomerBuilder.New().Build();
        //    var application = ApplicationBuilder.New(customer).Build();

        //    var command = new Wonga.QA.Framework.Cs.RevokeApplicationFromDcaCommand
        //    {
        //        ApplicationId = application.Id
        //    };

        //    // TODO: Need to write a Debt Collection record to the DB

        //    Drive.Cs.Commands.Post(command);

        //    // TODO: Need to query for a further debt collection record that indicates the revoke
        //}
    }
}