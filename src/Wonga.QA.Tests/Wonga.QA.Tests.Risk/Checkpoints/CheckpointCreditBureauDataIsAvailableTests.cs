using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	class CheckpointCreditBureauDataIsAvailable : BaseCheckpointTest
	{
		private const string TestMask = "test:CreditBureauDataIsAvailable";

		private string _forename = "ANITHA";
		private string _surname = "ESSACK";
		private Date _dateOfBirth = new Date(new DateTime(1957,12,19));
		private string _nationalNumber = "5712190106083";

		[Test, AUT(AUT.Za), JIRA("ZA-1910")]
		public void CheckpointCreditBureauDataIsAvailableAccepted()
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			var application = ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Uk), JIRA("UK-851")]
		public void DeclineIfNoData()
		{
			RunSingleWorkflowTest(TestMask, new CustomerJanetUk{ForeName = "NoData"}, CheckpointDefinitionEnum.CreditBureauDataIsAvailable, CheckpointStatus.Failed);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-851")]
		public void AcceptIfCallReport()
		{
			RunSingleWorkflowTest(TestMask, new CustomerJanetUk(), CheckpointDefinitionEnum.CreditBureauDataIsAvailable, CheckpointStatus.Failed);
		}
	}
}
