using System.Linq;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Salesforce
{
	public abstract class SalesforceTestBase
	{
		protected Framework.ThirdParties.Salesforce Salesforce;

		public SalesforceTestBase()
		{
			Salesforce = Drive.ThirdParties.Salesforce;

			var sfUsername = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
			var sfPassword = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
			var sfUrl = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");

			Salesforce.SalesforceUsername = sfUsername.Value;
			Salesforce.SalesforcePassword = sfPassword.Value;
			Salesforce.SalesforceUrl = sfUrl.Value;
		}
	}
}
