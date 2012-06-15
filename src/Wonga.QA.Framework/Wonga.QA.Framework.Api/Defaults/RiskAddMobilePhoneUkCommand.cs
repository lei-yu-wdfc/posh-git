using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskAddMobilePhoneUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			MobilePhone = "0210000000";
		}
	}
}
