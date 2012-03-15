using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class FeeTests
	{

		[Test, AUT(AUT.Za), Explicit("Release")]
		public void AdditionalServiceFeeCreatedForLoanOver30Days()
		{

		}

		[Test, AUT(AUT.Za), Explicit("Release")]
		public void AdditionalServiceFeeNotCreatedForLoanOver30Days()
		{

		}
	}
}
