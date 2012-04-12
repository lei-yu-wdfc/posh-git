using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Framework.Mocks.Entities
{
	public class ScotiaSetup
	{
		private readonly int _setupId;

		public ScotiaSetup(int setupId)
		{
			_setupId = setupId;
		}

		public void Verify()
		{
			using(var driver = new DbDriver().QaData)
			{
				Do.With.Message(string.Format("Setup bank gateway response {0} did not match any processed transaction", _setupId)).
					While(() => driver.BankGatewayResponseSetups.SingleOrDefault(s => s.BankGatewayResponseSetupId == _setupId && s.Hits == 0));
			}
		}
	}
}