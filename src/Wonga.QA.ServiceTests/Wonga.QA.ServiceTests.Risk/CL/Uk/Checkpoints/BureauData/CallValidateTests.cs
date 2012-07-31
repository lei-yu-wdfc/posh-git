using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq.Messages.Risk.CallValidate;
using Wonga.QA.ServiceTests.Risk.CL.uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.Uk.Checkpoints.BureauData
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class CallValidateTests:RiskServiceTestClUkBase
	{
		[Test]
		public void IfApplicantPaymentCardPassesCallValidate_LoanIsAccepted()
		{
			SetupLegitCustomer();
			WhenTheL0UserAppliesForALoan();
			ThenTheRiskServiceShouldApproveTheLoan();
		}

		#region Setup
		protected override void SetupLegitCustomer(System.DateTime? dateOfBirth = null)
		{
			base.SetupLegitCustomer(dateOfBirth);
			SetupCallValidateHappyCase();
		}

		private void SetupCallValidateHappyCase()
		{
			EndpointMock.AddHandler<CallValidateRequestMessage>(
				filter: x => x.ApplicationId == this.ApplicationId,
				action: (receivedMsg) =>
				        	{
				        		var response = GetCallValidateHappyCaseResponse();
				        		response.SagaId = receivedMsg.SagaId;
				        		Send(response);
				        	});
		}

		private CallValidateResponseMessage GetCallValidateHappyCaseResponse()
		{
			var xmlResponse = new DbDriver().QaData.CallValidateOutputs.First(x => x.ResponseType == "Success");
			return new CallValidateResponseMessage {Response = xmlResponse.Response};
		}

		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();

			Background(maskName: RiskMask.TESTCallValidatePaymentCardIsValid,
						checkpointName: "PaymentCardIsValid",
						responsibleVerification: "CallValidatePaymentCardIsValidVerification");

			CleanUp();
		}

		private void CleanUp()
		{
		}
		#endregion

		

	}
}
