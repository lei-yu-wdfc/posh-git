using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Commands
{
	[Parallelizable(TestScope.Self)]
	[TestFixture]    
	public class RiskSaveCustomerDetailsTests
	{
		private static readonly dynamic SagaEntity = Drive.Data.OpsSagas.Db.RiskSaveCustomerDetailsSagaEntity;
		
		[Test, AUT(AUT.Ca, AUT.Za)]
		public void WhenCommandCalledThenTheMobilePhoneShouldBeUpdatedOnRiskSaveCustomerDetailsSagaEntity()
		{
			var command = RiskSaveCustomerDetailsCommand.New();

			Guid accountId = (Guid)command.AccountId;
			string mobilePhone = command.MobilePhone.ToString();

			Drive.Api.Commands.Post(command);

			Do.Until(() => SagaEntity.FindBy(AccountId: accountId, MobilePhone: mobilePhone));
		}

	}
}
