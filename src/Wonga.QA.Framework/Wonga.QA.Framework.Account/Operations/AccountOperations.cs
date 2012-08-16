using Wonga.QA.Framework.Account.Operations.Business;
using Wonga.QA.Framework.Account.Operations.Consumer;
using Wonga.QA.Framework.Account.Operations.PayLater;

namespace Wonga.QA.Framework.Account.Operations
{
	public static class AccountOperations
	{
		public static BusinessAccountOperations Business = new BusinessAccountOperations();
		public static ConsumerAccountOperations Consumer = new ConsumerAccountOperations();
		public static PayLaterAccountOperations PayLater = new PayLaterAccountOperations();
	}
}
