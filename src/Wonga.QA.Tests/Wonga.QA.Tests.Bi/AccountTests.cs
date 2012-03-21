﻿using System;
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
		public void AccountDetailsStoredInBiTable()
		{
			var customer = CustomerBuilder.New().Build();
			Do.Until(() => Drive.Db.Bi.Accounts.Any(a => a.AccountNKey == customer.Id));
		}
	}
}
