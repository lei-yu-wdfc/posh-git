using Wonga.QA.Framework.Application.Business;
using Wonga.QA.Framework.Application.Consumer;
using Wonga.QA.Framework.Application.PayLater;

namespace Wonga.QA.Framework.Application
{
	public static class ApplicationOperations
	{
		public static BusinessApplicationOperations Business = new BusinessApplicationOperations();
		public static ConsumerApplicationOperations Consumer = new ConsumerApplicationOperations();
		public static PayLaterApplicationOperations PayLater = new PayLaterApplicationOperations();
	}
}
