using Wonga.QA.Framework.Account.Queries.Business;
using Wonga.QA.Framework.Account.Queries.Consumer;
using Wonga.QA.Framework.Account.Queries.PayLater;

namespace Wonga.QA.Framework.Account.Queries
{
	public static class AccountQueries
	{
		public static BusinessAccountQueries Business = new BusinessAccountQueries();
		public static ConsumerAccountQueries Consumer = new ConsumerAccountQueries();
		public static PayLaterAccountQueries PayLater = new PayLaterAccountQueries();
	}
}
