using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Bi
{
	[TestFixture]
	class AccountTests
	{
		[Test]
		public void AccountDetailsStoredInTable()
		{
			var customer = CustomerBuilder.New().Build();

			Do.Until(() => Driver.Db.Bi.Accounts.Single(a => a.AccountNKey == customer.Id));
		}
	}
}
