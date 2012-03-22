using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class CsapiRevokeApplicationFromDcaTest
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationHasNotBeenMovedToDca()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var command = new Wonga.QA.Framework.Cs.RevokeApplicationFromDcaCommand
                                               {
												   ApplicationId = application.Id
                                               };

        	try
        	{
				Drive.Cs.Commands.Post(command);
			}
        	catch (ValidatorException exception)
        	{
				Assert.AreEqual("Payments_RevokeFromDca_ApplicationHasNotBeenMovedToDca", exception.Errors.Single());
        	}
        }

		[Test, AUT(AUT.Ca), JIRA("CA-1862")]
		public void ShouldRejectCommandBecauseApplicationWasNotSpecified()
		{
			var command = new Wonga.QA.Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = Guid.Empty
			};

			try
			{
				Drive.Cs.Commands.Post(command);
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
