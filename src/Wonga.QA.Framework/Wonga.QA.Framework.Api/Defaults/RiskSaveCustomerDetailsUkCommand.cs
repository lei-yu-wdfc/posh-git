using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskSaveCustomerDetailsUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			Forename = Get.GetName();
			Surname = Get.GetName();
			Email = Get.RandomEmail();
			DateOfBirth = Get.GetDoB();
			Gender = Get.RandomEnum<GenderEnum>();
			MiddleName = Get.GetName();
			HomePhone = "0210000000";
			WorkPhone = "0210000000";
		}
	}
}
