using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Sms
{
	public class ClickatellIntegrationTests
	{
		private readonly dynamic _smsMessages = Drive.Data.Sms.Db.SmsMessages;
		private bool _smsMockEnabled;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_smsMockEnabled = Drive.Data.Ops.SetServiceConfiguration(
				Get.EnumToString(ServiceConfigurationKeys.MocksSmsEnabled),
				false,
				true);
		}

		[FixtureTearDown]
		public void FixtureTeardown()
		{
			Drive.Data.Ops.SetServiceConfiguration(
				Get.EnumToString(ServiceConfigurationKeys.MocksSmsEnabled),
				_smsMockEnabled);
		}

		[Test, JIRA("ZA-2414"), AUT(AUT.Za)]
		public void ClickatellIntegrationIsWorkingForZa()
		{
			const string phoneNumber = "27999900001";

			IntegrationIsWorking(phoneNumber);
		}

		private void IntegrationIsWorking(string phoneNumber)
		{
			string smsText = Guid.NewGuid().ToString();
			Guid smsId = StoreSms(phoneNumber, smsText);

			SendSms(smsId, phoneNumber, smsText);

			Do.Until(() => _smsMessages.FindByExternalId(smsId).Status == 3);
		}

		private Guid StoreSms(string phoneNumber, string smsText)
		{
			Guid smsId = Guid.NewGuid();

			_smsMessages.Insert(ExternalId: smsId, Status: 0, MobilePhoneNumber: phoneNumber, MessageText: smsText);

			return smsId;
		}

		private static void SendSms(Guid smsId, string phoneNumber, string smsText)
		{
			Drive.Msmq.Sms.Send(new SendSmsSmsCommand
			                    	{
			                    		FormattedPhoneNumber = phoneNumber,
			                    		SmsMessageId = smsId,
			                    		Text = smsText
			                    	});
		}
	}
}
