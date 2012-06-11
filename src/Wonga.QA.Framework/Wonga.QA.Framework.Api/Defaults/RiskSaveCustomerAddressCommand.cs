﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskSaveCustomerAddressCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			AddressId = Get.GetId();
			County = Get.RandomString(10);
			HouseNumber = Get.RandomInt(1, 1000);
			Postcode = "0300";
			Street = "Street";
			Town = "City";
			SubRegion = "Sub";
		}
	}
}
