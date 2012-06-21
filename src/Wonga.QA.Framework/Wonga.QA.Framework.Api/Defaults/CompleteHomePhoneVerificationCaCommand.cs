using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca
{
	public partial class CompleteHomePhoneVerificationCaCommand
	{
		public override void Default()
		{
			VerificationId = Get.GetId();
			Pin = "0000";
		}
	}
}
