using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public class PayLaterAccountDataBase
	{
		public String Password;
		public String Email;

		public PayLaterAccountDataBase()
		{
			Password = Get.GetPassword();
			Email = Get.RandomEmail();
		}
	}
}
