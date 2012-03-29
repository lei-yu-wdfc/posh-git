using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Sms
{
    class MbloxIntegrationTests
    {
        private ServiceConfigurationEntity _configurationEntity = null;

        private const string TEST_PHONE_NUMBER = "4407786777486";
        private const string TEST_MESSAGE_TEXT = "Testing request for sms message send";
        private const string RESERVE_PROVIDER_KEY = "SMS.MBLOX_USE_RESERVE";
        private const string USE_ANOTHER_PROVIDER = "true";
        private const string USE_MBLOX_PROVIDER = "false";

        [SetUp]
        public void init()
        {
            _configurationEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key.Equals(MbloxIntegrationTests.RESERVE_PROVIDER_KEY));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-510")]
        public void SendRequestToMbloxProvider()
        {
            var request = new SendSimpleSmsCommand()
            {
                MessageText = MbloxIntegrationTests.TEST_MESSAGE_TEXT,
                ToNumber = MbloxIntegrationTests.TEST_PHONE_NUMBER
            };
            Drive.Msmq.Sms.Send(request);
            var messages =
                Do.Until(() => Drive.Db.Sms.SmsMessages.Where(x => x.MessageText.Equals(request.MessageText)).OrderByDescending(x => x.SmsMessageId));
            var lastMessage = messages.First();

            Assert.IsTrue(lastMessage.MobilePhoneNumber.Equals(request.ToNumber));
            Assert.IsTrue(lastMessage.MessageText.Equals(request.MessageText));
            Assert.IsNull(lastMessage.ErrorMessage);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-510"),]
        public void SwitchToAnotherProviderAndSendSmsRequest()
        {

            if (_configurationEntity.Value.Equals(MbloxIntegrationTests.USE_MBLOX_PROVIDER))
            {
                _configurationEntity.Value = MbloxIntegrationTests.USE_ANOTHER_PROVIDER;
                _configurationEntity.Submit();
            }

            var request = new SendSimpleSmsCommand()
            {
                MessageText = MbloxIntegrationTests.TEST_MESSAGE_TEXT,
                ToNumber = MbloxIntegrationTests.TEST_PHONE_NUMBER
            };
            Drive.Msmq.Sms.Send(request);
            var messages = Do.Until(() => Drive.Db.Sms.SmsMessages.Where(x => x.MessageText.Equals(request.MessageText)).OrderByDescending(x => x.SmsMessageId));
            var lastMessage = messages.First();

            Assert.IsTrue(_configurationEntity.Value.Equals(MbloxIntegrationTests.USE_ANOTHER_PROVIDER));
            Assert.IsTrue(lastMessage.MobilePhoneNumber.Equals(request.ToNumber));
            Assert.IsTrue(lastMessage.MessageText.Equals(request.MessageText));
            Assert.IsNull(lastMessage.ErrorMessage);
        }
    }
}
