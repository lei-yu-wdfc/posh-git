using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca
{
	public partial class RiskSaveCustomerDetailsCaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			Forename = Get.GetName();
			Surname = Get.GetName();
			Email = Get.RandomEmail();
			DateOfBirth = Get.GetDoB();
			Gender = Get.RandomEnum<GenderEnum>();
			HomePhone = "0210000000";
			WorkPhone = "0210000000";
			MobilePhone = "0210000000";

			if ((GenderEnum)Gender != GenderEnum.Female)
				MaidenName = null;
		}
	}
}
