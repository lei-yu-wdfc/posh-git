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
        private const string MBLOX_MESSAGE_TEXT_TEST = "Sending test message from acceptance tests with using Mblox provider with GUID : ";
        private const string ZONG_MESSAGE_TEXT_TEST = "Sending test message from acceptance tests with using Zong provider with GUID :" ;
        private const string PROVIDER_CHOOSE_KEY = "SMS.PROVIDER_NAME";
        private const string ZONG_PROVIDER_NAME = "ZONG";
        private const string MBLOX_PROVIDER_NAME = "MBLOX";

        [SetUp]
        public void Init()
        {
            _configurationEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key.Equals(MbloxIntegrationTests.PROVIDER_CHOOSE_KEY));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-510")]
        public void SendRequestToMbloxProvider()
        {
            var request = new SendSimpleSmsCommand()
            {
                MessageText = MbloxIntegrationTests.MBLOX_MESSAGE_TEXT_TEST + Guid.NewGuid(),
                ToNumber = MbloxIntegrationTests.TEST_PHONE_NUMBER
            };
            Drive.Msmq.Sms.Send(request);
            var message = Do.Until(() => Drive.Db.Sms.SmsMessages.Where(x => x.MessageText.Equals(request.MessageText)).Single());

            Assert.IsTrue(message.MobilePhoneNumber.Equals(request.ToNumber));
            Assert.IsTrue(message.MessageText.Equals(request.MessageText));
            Assert.IsTrue(_configurationEntity.Value.Equals(MbloxIntegrationTests.MBLOX_PROVIDER_NAME));
            Assert.IsNull(message.ErrorMessage);
            Assert.IsNotNull(message);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-510"),]
        public void SwitchToAnotherProviderAndSendSmsRequest()
        {

            if (!_configurationEntity.Value.Equals(MbloxIntegrationTests.ZONG_PROVIDER_NAME))
            {
                _configurationEntity.Value = MbloxIntegrationTests.ZONG_PROVIDER_NAME;
                _configurationEntity.Submit();
            }

            var request = new SendSimpleSmsCommand()
            {
                MessageText = MbloxIntegrationTests.ZONG_MESSAGE_TEXT_TEST + Guid.NewGuid(),
                ToNumber = MbloxIntegrationTests.TEST_PHONE_NUMBER
            };
            Drive.Msmq.Sms.Send(request);
            var message = Do.Until(() => Drive.Db.Sms.SmsMessages.Where(x => x.MessageText.Equals(request.MessageText)).Single());

            Assert.IsTrue(_configurationEntity.Value.Equals(MbloxIntegrationTests.ZONG_PROVIDER_NAME));
            Assert.IsTrue(message.MobilePhoneNumber.Equals(request.ToNumber));
            Assert.IsTrue(message.MessageText.Equals(request.MessageText));
            Assert.IsNull(message.ErrorMessage);
            Assert.IsNotNull(message);
        }
        [TearDown,AUT(AUT.Uk)]
        public void RollbackOptions()
        {
            if (_configurationEntity.Value.Equals(MbloxIntegrationTests.ZONG_PROVIDER_NAME))
            {
                _configurationEntity.Value = MbloxIntegrationTests.MBLOX_PROVIDER_NAME;
                _configurationEntity.Submit();
            }
                
        }
    }
}
