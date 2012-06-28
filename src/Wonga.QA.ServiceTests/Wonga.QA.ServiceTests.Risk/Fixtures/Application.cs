
using System;

namespace Wonga.QA.ServiceTests.Risk.Fixtures
{
	public class Application
	{
		protected Guid ApplicationId { get; private set; }

		public Application()
		{
			ApplicationId = Guid.NewGuid();


		}
	}
}
