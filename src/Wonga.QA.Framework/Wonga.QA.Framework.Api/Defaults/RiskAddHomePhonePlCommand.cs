using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskAddHomePhonePlCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			HomePhone = "0210000000";
		}
	}
}
