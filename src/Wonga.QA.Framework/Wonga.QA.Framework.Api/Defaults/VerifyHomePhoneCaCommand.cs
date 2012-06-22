﻿using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca
{
	public partial class VerifyHomePhoneCaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			VerificationId = Get.GetId();
			HomePhone = "0720000000";
			Forename = "Forename";
		}
	}
}
