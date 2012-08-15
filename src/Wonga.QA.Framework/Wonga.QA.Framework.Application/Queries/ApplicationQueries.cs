using Wonga.QA.Framework.Application.Business;
using Wonga.QA.Framework.Application.Queries.Consumer;
using Wonga.QA.Framework.Application.Queries.PayLater;

namespace Wonga.QA.Framework.Application.Queries
{
	public static class ApplicationQueries
	{
		public static BusinessApplicationQueries Business = new BusinessApplicationQueries();
		public static ConsumerApplicationQueries Consumer = new ConsumerApplicationQueries();
		public static PayLaterApplicationQueries PayLater = new PayLaterApplicationQueries();
	}
}
