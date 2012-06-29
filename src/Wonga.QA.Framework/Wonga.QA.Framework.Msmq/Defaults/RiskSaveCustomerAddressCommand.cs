using System;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
	public partial class RiskSaveCustomerAddressCommand
	{
		public override void Default()
		{
			AddressId = AddressId;
			HouseNumber = "1";
			Postcode = "NW1 7SN";
			Street = "Prince Albert Road";
			Town = "London";
			County = "UK";
			HouseName = "1";
			Flat = "1";
			District = "1";
		}
	}
}
