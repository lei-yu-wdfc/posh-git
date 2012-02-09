using System;
using System.Diagnostics;
using System.Messaging;
using System.Security.Principal;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    public class MsmqQueue
    {
        private MessageQueue _queue;
        private MessageQueue _journal;
        private MessageQueue _error;
        private MessageQueue _subscription;

        private MessageQueue _response;
        private MessageQueue _administration;

        public MsmqQueue(String path)
        {
            _queue = new MessageQueue(path);
            _journal = new MessageQueue(String.Format("{0};journal", _queue.Path));
            _error = new MessageQueue(String.Format("{0}_error", _queue.Path));
            _subscription = new MessageQueue(String.Format("{0}_subscription", _queue.Path));

            _response = new MessageQueue(String.Format("{0}\\qa.response", _queue.Path.Substring(0, _queue.Path.LastIndexOf('\\'))));
            _administration = new MessageQueue(String.Format("{0}\\qa.administration", _queue.Path.Substring(0, _queue.Path.LastIndexOf('\\'))));
        }

        static MsmqQueue()
        {
            if (Config.SUT == SUT.Dev)
            {
                String response = @".\private$\qa.response";
                String administration = @".\private$\qa.administration";
                String user = new SecurityIdentifier(WellKnownSidType.WorldSid, null).Translate(typeof(NTAccount)).ToString();

                if (!MessageQueue.Exists(response))
                    MessageQueue.Create(response, true).SetPermissions(user, MessageQueueAccessRights.GenericRead | MessageQueueAccessRights.GenericWrite);

                if (!MessageQueue.Exists(administration))
                    MessageQueue.Create(administration).SetPermissions(user, MessageQueueAccessRights.GenericRead | MessageQueueAccessRights.GenericWrite);
            }
        }

        public void Send(MsmqMessage message)
        {
            String body = message.ToString();
            Trace.WriteLine(Data.Indent(body), GetType().FullName);

            Byte[] bytes = Encoding.Default.GetBytes(body);
            Message send = new Message
            {
                ResponseQueue = _response,
                AdministrationQueue = _administration,
                AcknowledgeType = AcknowledgeTypes.FullReceive
            };
            send.BodyStream.Write(bytes, 0, bytes.Length);
            _queue.Send(send, MessageQueueTransactionType.Single);
        }

        private MsmqQueue Wait(MsmqMessage message)
        {
            throw new NotImplementedException();
        }

        private MsmqMessage Find(Guid id)
        {
            throw new NotImplementedException();
        }

        private MsmqQueue Purge()
        {
            throw new NotImplementedException();
        }
    }
}
