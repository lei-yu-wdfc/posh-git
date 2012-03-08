using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Blacklist;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointApplicationElementNotOnBlacklistTests
	{
		private const string TestMask = "test:Blacklist";

		private string _internationalCode;
		private const string InternationalCodeZa = "+27";

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_internationalCode = InternationalCodeZa;
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnBlacklistAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithBankAccountNumber(Data.GetBankAccountNumber()).Build();
			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnBlacklistMobilePhoneNumberPresent()
		{
			var mobilePhoneNumber = Data.GetMobilePhone();
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithBankAccountNumber(Data.GetBankAccountNumber())
				.WithMobileNumber(mobilePhoneNumber).Build();
			
			var formattedMobilePhoneNumber = _internationalCode + mobilePhoneNumber.Remove(0,1);

			var blacklistEntity = new BlackListEntity{MobilePhone = formattedMobilePhoneNumber, ExternalId =  Guid.NewGuid()};
			Driver.Db.Blacklist.BlackLists.InsertOnSubmit(blacklistEntity);
			blacklistEntity.Submit();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnBlacklistBankAccountPresent()
		{
			var bankAccountNumber = Data.GetBankAccountNumber();
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithBankAccountNumber(Data.GetBankAccountNumber()).Build();
			var blacklistEntity = new BlackListEntity { BankAccount = bankAccountNumber.ToString(), ExternalId = Guid.NewGuid() };
			Driver.Db.Blacklist.BlackLists.InsertOnSubmit(blacklistEntity);
			blacklistEntity.Submit();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}
	}
}
