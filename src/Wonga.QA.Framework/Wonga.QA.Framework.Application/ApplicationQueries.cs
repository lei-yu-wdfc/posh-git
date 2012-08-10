﻿using Wonga.QA.Framework.Application.Business;
using Wonga.QA.Framework.Application.Consumer;
using Wonga.QA.Framework.Application.PayLater;

namespace Wonga.QA.Framework.Application
{
	public static class ApplicationQueries
	{
		public static BusinessApplicationQueries Business = new BusinessApplicationQueries();
		public static ConsumerApplicationQueries Consumer = new ConsumerApplicationQueries();
		public static PayLaterApplicationQueries PayLater = new PayLaterApplicationQueries();
	}
}
