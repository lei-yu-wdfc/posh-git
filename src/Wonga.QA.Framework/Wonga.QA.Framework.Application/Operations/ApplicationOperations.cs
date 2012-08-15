using Wonga.QA.Framework.Application.Operations.Business;
using Wonga.QA.Framework.Application.Operations.Consumer;
using Wonga.QA.Framework.Application.Operations.PayLater;

namespace Wonga.QA.Framework.Application.Operations
{
	public static class ApplicationOperations
	{
		public static BusinessApplicationOperations Business = new BusinessApplicationOperations();
		public static ConsumerApplicationOperations Consumer = new ConsumerApplicationOperations();
		public static PayLaterApplicationOperations PayLater = new PayLaterApplicationOperations();
	}
}
