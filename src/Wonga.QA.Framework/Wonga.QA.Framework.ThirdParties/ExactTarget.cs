﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Wonga.QA.Framework.ThirdParties.ExactTargetService;

namespace Wonga.QA.Framework.ThirdParties
{
    public class ExactTarget
    {
        private SoapClient _soapClient;

        private const string ExactTargetServiceUrlString = "https://webservice.test.exacttarget.com/Service.asmx";
        private readonly Uri _exactTargetServiceUrl;
        public ExactTarget()
        {
            Binding binding = ConstructBinding();
            _exactTargetServiceUrl = new Uri(ExactTargetServiceUrlString);
            _soapClient = new SoapClient(binding, new EndpointAddress(_exactTargetServiceUrl));
            _soapClient.ClientCredentials.UserName.UserName = "x188167_Admin";
            _soapClient.ClientCredentials.UserName.Password = "welcome@1";
        }

        private Binding ConstructBinding()
        {
            //NOTE: based on ET API documentation sample
            //http://docs.code.exacttarget.com/020_Web_Service_Guide/Getting_Started_Developers_and_the_ExactTarget_API/Connecting_to_the_Web_Service_API_using_WCF
            var binding = new BasicHttpBinding();
            binding.Name = "UserNameSoapBinding";
            binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
            binding.OpenTimeout = TimeSpan.FromMinutes(5);
            binding.CloseTimeout = TimeSpan.FromMinutes(5);
            binding.SendTimeout = TimeSpan.FromMinutes(5);

            return binding;
        }

        public bool CheckPaymentReminderEmailSent(string emailAddress)
        {
            const string emailTemplateId = "32377";

            return CheckEmailSent(emailAddress, emailTemplateId);
        }

        public bool CheckPaymentConfirmationUkEmailSent(string emailAddress)
        {
            const string emailTemplateId = "34009";

            return CheckEmailSent(emailAddress, emailTemplateId);
        }

        //SH: Leaving this as public so it can be used directly.
        public bool CheckEmailSent(string emailAddress, string templateId)
        {
            TriggeredSendDefinition triggeredSendDefinition = GetTriggeredSendDefinition(templateId);

            if (triggeredSendDefinition == null)
            {
                throw new Exception("Could not find type for PaymentReminderEmail");
            }

            SentEvent sentEvent = GetSentEvent(emailAddress, triggeredSendDefinition);

            return sentEvent != null;
        }

        private TriggeredSendDefinition GetTriggeredSendDefinition(string customerKey)
        {
            var filter = new SimpleFilterPart
            {
                Property = "CustomerKey",
                Value = new[] { customerKey },
                SimpleOperator = SimpleOperators.equals
            };

            var request = new RetrieveRequest
            {
                ObjectType = "TriggeredSendDefinition",
                Filter = filter,
                Properties = new[] { "ObjectID", "Name" }
            };

            string requestId;
            APIObject[] results;

            string response = _soapClient.Retrieve(request, out requestId, out results);

            if (response == "OK" && results.Length > 0)
            {
                return results[0] as TriggeredSendDefinition;
            }

            return null;
        }

        private SentEvent GetSentEvent(string emailAddress, TriggeredSendDefinition triggeredSendDefinition)
        {
            var filter = new ComplexFilterPart
            {
                LeftOperand =
                    new SimpleFilterPart
                    {
                        Property = "SubscriberKey",
                        Value = new[] { emailAddress },
                        SimpleOperator = SimpleOperators.@equals
                    },
                RightOperand =
                    new SimpleFilterPart
                    {
                        Property = "TriggeredSendDefinitionObjectID",
                        Value = new[] { triggeredSendDefinition.ObjectID },
                        SimpleOperator = SimpleOperators.@equals
                    },
                LogicalOperator = LogicalOperators.AND
            };

            var request = new RetrieveRequest
            {
                ObjectType = "SentEvent",
                Filter = filter,
                Properties =
                    new[]
                                          {
                                              "EventType", "EventDate", "SendID", "SubscriberKey",
                                              "TriggeredSendDefinitionObjectID"
                                          }
            };

            string requestId;
            APIObject[] results;

            string response = _soapClient.Retrieve(request, out requestId, out results);

            if (response == "OK" && results.Length > 0)
            {
                return results[0] as SentEvent;
            }

            return null;
        }

        private TrackingEvent GetTrackingEvent(string emailAddress, TriggeredSendDefinition triggeredSendDefinition)
        {
            var filter = new ComplexFilterPart
            {
                LeftOperand =
                    new SimpleFilterPart
                    {
                        Property = "SubscriberKey",
                        Value = new[] { emailAddress },
                        SimpleOperator = SimpleOperators.@equals
                    },
                RightOperand =
                    new SimpleFilterPart
                    {
                        Property = "TriggeredSendDefinitionObjectID",
                        Value = new[] { triggeredSendDefinition.ObjectID },
                        SimpleOperator = SimpleOperators.@equals
                    },
                LogicalOperator = LogicalOperators.AND
            };

            var request = new RetrieveRequest
            {
                ObjectType = "TrackingEvent",
                Filter = filter,
                Properties =
                    new[]
                                          {
                                              "EventType", "EventDate", "SendID", "SubscriberKey",
                                              "TriggeredSendDefinitionObjectID"
                                          }
            };

            string requestId;
            APIObject[] results;

            string response = _soapClient.Retrieve(request, out requestId, out results);

            if (response == "OK" && results.Length > 0)
            {
                return results[0] as TrackingEvent;
            }

            return null;
        }

        private TriggeredSend GetTriggeredSend(SentEvent sentEvent, TriggeredSendDefinition triggeredSendDefinition)
        {
            var filter = new SimpleFilterPart
                             {
                                 Property = "Id",
                                 Value = new[] {sentEvent.SendID.ToString()},
                                 SimpleOperator = SimpleOperators.equals
                             };

            var request = new RetrieveRequest
                              {
                                  ObjectType = "TriggeredSend",
                                  Filter = filter,
                                  Properties =
                                      new[]
                                          {
                                              "Subscribers", "Attributes"
                                          }
                              };

            string requestId;
            APIObject[] results;

            string response = _soapClient.Retrieve(request, out requestId, out results);

            if (response == "OK" && results.Length > 0)
            {
                return results[0] as TriggeredSend;
            }

            return null;
        }

        private Subscriber GetSubscriber(string emailAddress)
        {
            var filter = new SimpleFilterPart
                             {
                                 Property = "SubscriberKey",
                                 Value = new[] { emailAddress },
                                 SimpleOperator = SimpleOperators.equals
                             };

            var request = new RetrieveRequest
                              {
                                  ObjectType = "Subscriber",
                                  Filter = filter,
                                  Properties = new[] { "SubscriberKey" }
                              };

            string requestId;
            APIObject[] results;

            string response = _soapClient.Retrieve(request, out requestId, out results);

            if (response == "OK" && results.Length > 0)
            {
                return results[0] as Subscriber;
            }

            return null;
        }

        public bool CheckPaymentReceiptEmailSent(string emailAddress)
        {
            const string emailTemplateId = "20932";

            TriggeredSendDefinition triggeredSendDefinition = GetTriggeredSendDefinition(emailTemplateId);

            if (triggeredSendDefinition == null)
            {
                throw new Exception("Could not find type for PaymentReminderEmail");
            }

            SentEvent sentEvent = GetSentEvent(emailAddress, triggeredSendDefinition);

            if (sentEvent == null)
            {
                return false;
            }

            return true;
        }

    }
}