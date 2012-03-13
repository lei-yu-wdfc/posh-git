using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Api
{
	[Parallelizable(TestScope.All)]
	public class ApiApplicationTests
	{
		[Test, AUT(AUT.Za)]
		public void AccountWithOpenApplicationCannotCreateAnother()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build();

			ApplicationBuilder.New(customer).Build();
		}
		
	
	}
}