using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, Parallelizable(TestScope.All), Pending("Transunion TC delayed response")]
	class CheckpointCreditBureauDataIsAvailable
	{
        private const RiskMask TestMask = RiskMask.TESTCreditBureauDataIsAvailable;

		private string _forename = "ANITHA";
		private string _surname = "ESSACK";
		private Date _dateOfBirth = new Date(new DateTime(1957,12,19));
		private string _nationalNumber = "5712190106083";

		[Test, AUT(AUT.Za), JIRA("ZA-1910"), Pending("Transunion TC delayed response")]
		public void DataAvailableIsAccepted()
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}
		
	}
}
