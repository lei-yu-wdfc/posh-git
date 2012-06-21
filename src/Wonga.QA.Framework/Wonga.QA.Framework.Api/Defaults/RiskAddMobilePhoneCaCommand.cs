using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca
{
	public partial class RiskAddMobilePhoneCaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			MobilePhone = "0210000000";
		}
	}
}
