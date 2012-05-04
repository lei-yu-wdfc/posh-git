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
        //private ServiceConfigurationEntity _configurationEntity = null;

        private const string TEST_PHONE_NUMBER = "447786777486";
        private const string MBLOX_MESSAGE_TEXT_TEST = "Sending test message from acceptance tests with using Mblox provider with GUID : ";
        private const string ZONG_MESSAGE_TEXT_TEST = "Sending test message from acceptance tests with using Zong provider with GUID :";
        private const string PROVIDER_CHOOSE_KEY = "Sms.Provider_name";
        private const string ZONG_PROVIDER_NAME = "Zong";
        private const string MBLOX_PROVIDER_NAME = "Mblox";

        private static readonly dynamic _serviceConfigurationsEntity = Drive.Data.Ops.Db.ServiceConfigurations;
        private static readonly dynamic _smsEntity = Drive.Data.Sms.Db.SmsMessages;

        [Test, AUT(AUT.Uk), JIRA("UK-510")]
        public void SendRequestToMbloxProvider()
        {

            var request = new SendSmsCommsSmsCommand
            {
                MessageText = MBLOX_MESSAGE_TEXT_TEST + "" + Guid.NewGuid(),
                OriginatingSagaId = Guid.NewGuid(),
                ToNumberFormatted = TEST_PHONE_NUMBER
            };

            Drive.Msmq.SmsDistrubutor.Send(request);
            var message = Do.Until(() => _smsEntity.FindBy(MessageText: request.MessageText));


            Assert.IsTrue(message.MobilePhoneNumber.Equals(request.ToNumberFormatted));
            Assert.IsTrue(message.MessageText.Equals(request.MessageText));
            Assert.IsNull(message.ErrorMessage);
            Do.Until(() => _serviceConfigurationsEntity.FindBy(Key:PROVIDER_CHOOSE_KEY, Value:MBLOX_PROVIDER_NAME));

        }

        [Test, AUT(AUT.Uk), JIRA("UK-510"),]
        public void SwitchToAnotherProviderAndSendSmsRequest()
        {
           changeProvider();

            var request = new SendSmsCommsSmsCommand
            {
                MessageText = ZONG_MESSAGE_TEXT_TEST + "" + Guid.NewGuid(),
                OriginatingSagaId = Guid.NewGuid(),
                ToNumberFormatted = TEST_PHONE_NUMBER
            };

            Drive.Msmq.SmsDistrubutor.Send(request);
            var message = Do.Until(() => _smsEntity.FindBy(MessageText: request.MessageText));

            Assert.IsTrue(message.MobilePhoneNumber.Equals(request.ToNumberFormatted));
            Assert.IsTrue(message.MessageText.Equals(request.MessageText));
            Assert.IsNull(message.ErrorMessage);
            Do.Until(() => _serviceConfigurationsEntity.FindBy(Key:PROVIDER_CHOOSE_KEY, Value:ZONG_PROVIDER_NAME));

            changeProvider();
        }


        private void changeProvider()
        {
            var provider_name = Do.Until(() => _serviceConfigurationsEntity.FindBy(Key:PROVIDER_CHOOSE_KEY));
            var providerName = provider_name.Value;
            if (providerName.Equals(ZONG_PROVIDER_NAME))
            {
                Do.Until(() => _serviceConfigurationsEntity.UpdateByServiceConfigurationId(
                     ServiceConfigurationId:provider_name.ServiceConfigurationId,
                     Key:PROVIDER_CHOOSE_KEY, Value:MBLOX_PROVIDER_NAME));
            }

            if (providerName.Equals(MBLOX_PROVIDER_NAME))
            {
                Do.Until(() => _serviceConfigurationsEntity.UpdateByServiceConfigurationId(
                     ServiceConfigurationId:provider_name.ServiceConfigurationId,
                     Key:PROVIDER_CHOOSE_KEY, Value:ZONG_PROVIDER_NAME));
            }
        }
    }
}
