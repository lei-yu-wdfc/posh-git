
using System;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
	public partial class RiskSaveCustomerDetails
	{
		public override void Default()
		{
			DateOfBirth = new DateTime(1990, 08, 09);
			Forename = "John";
			HomePhone = "0207050520";
			MiddleName = "Arnie";
			Surname = "Conor";
			WorkPhone = "0207450510";
		}
	}
}
