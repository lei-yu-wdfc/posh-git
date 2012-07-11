using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Meta
{
	[TestFixture]
	public class ResetConfiguration
	{
		[Test, AUT(AUT.Ca), Category(TestCategories.CoreTest)]
		public void ResetConfigurationCa()
		{
			Drive.Data.Ops.SetServiceConfiguration("BankGateway.Scotiabank.FileTransferTimes", string.Empty);
			Drive.Data.Ops.SetServiceConfiguration("BankGateway.Bmo.FileTransferTimes", string.Empty);
			Drive.Data.Ops.SetServiceConfiguration("BankGateway.Rbc.FileTransferTimes", string.Empty);
            Drive.Data.Ops.SetServiceConfiguration("BankGateway.IsTestMode", false);
			Drive.Data.Ops.SetServiceConfiguration("Payments.DelayBeforeApplicationClosedInMinutes", "0");
		}
	}
}
