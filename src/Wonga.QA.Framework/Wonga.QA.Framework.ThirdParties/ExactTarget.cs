using System;
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
            var elementCollection = new BindingElementCollection();
            var httpsTransport = new HttpsTransportBindingElement();
            elementCollection.Add(httpsTransport);

            var textMessageEncoding = new TextMessageEncodingBindingElement();
            textMessageEncoding.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
            elementCollection.Add(textMessageEncoding);

            SecurityBindingElement security =
                SecurityBindingElement.CreateSecureConversationBindingElement(
                    SecurityBindingElement.CreateUserNameOverTransportBindingElement());

            elementCollection.Add(security);

            var result = new CustomBinding(elementCollection)
                             {
                                 CloseTimeout = TimeSpan.FromMinutes(1),
                                 OpenTimeout = TimeSpan.FromMinutes(1),
                                 ReceiveTimeout = TimeSpan.FromMinutes(10),
                                 SendTimeout = TimeSpan.FromMinutes(4)
                             };
            return result;
        }

        public bool CheckPaymentReminderEmailSent(string emailAddress)
        {
            const string emailTemplateId = "32377";

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