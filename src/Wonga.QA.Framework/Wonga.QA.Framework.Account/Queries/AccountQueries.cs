using System;
using Wonga.QA.Framework.Account.Business;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Account.PayLater;

namespace Wonga.QA.Framework.Account.Queries
{
	public static class AccountQueries
	{
		public static BusinessAccountQueries Business = new BusinessAccountQueries();
		public static ConsumerAccountQueries Consumer = new ConsumerAccountQueries();
		public static PayLaterAccountQueries PayLater = new PayLaterAccountQueries();
	}
}
