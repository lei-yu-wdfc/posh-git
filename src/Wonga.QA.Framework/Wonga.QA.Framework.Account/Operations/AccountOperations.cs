using Wonga.QA.Framework.Account.Business;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Account.PayLater;

namespace Wonga.QA.Framework.Account.Operations
{
	public static class AccountOperations
	{
		public static BusinessAccountOperations Business = new BusinessAccountOperations();
		public static ConsumerAccountOperations Consumer = new ConsumerAccountOperations();
		public static PayLaterAccountOperations PayLater = new PayLaterAccountOperations();
	}
}
