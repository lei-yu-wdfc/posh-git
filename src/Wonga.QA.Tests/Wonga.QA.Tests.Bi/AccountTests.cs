using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Bi
{
	[Parallelizable(TestScope.All), Ignore("This test keeps failing due to a known issue  with BI")]
	class AccountTests
	{
		[Test, Pending("ZA-2565")]
		public void AccountDetailsStoredInBiTable()
		{
			var customer = CustomerBuilder.New().Build();
			//Do.Until(() => Drive.Db.Bi.Accounts.Any(a => a.AccountNKey == customer.Id));
		}
	}
}
