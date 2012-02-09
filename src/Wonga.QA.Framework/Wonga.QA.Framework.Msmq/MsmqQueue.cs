using System;
using System.Diagnostics;
using System.Messaging;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    public class MsmqQueue
    {
        private MessageQueue _queue;
        public MsmqQueue Journal { get; set; }
        public MsmqQueue Error { get; set; }
        public MsmqQueue Subscription { get; set; }

        public MsmqQueue(String path)
        {
            _queue = new MessageQueue(path);
            Journal = new MsmqQueue("{0}\\journal$", path);
            Error = new MsmqQueue("{0}_error", path);
            Subscription = new MsmqQueue("{0}_subscription", path);
        }

        private MsmqQueue(String format, String path)
        {
            _queue = new MessageQueue(String.Format(format, path));
        }

        public void Send(MsmqMessage message)
        {
            String body = message.ToString();
            Trace.WriteLine(Data.Indent(body), GetType().FullName);

            Byte[] bytes = Encoding.Default.GetBytes(body);
            Message m = new Message();
            m.BodyStream.Write(bytes, 0, bytes.Length);
            _queue.Send(m, MessageQueueTransactionType.Single);
        }

        public MsmqQueue Wait(MsmqMessage message)
        {
            throw new NotImplementedException();
        }

        public MsmqMessage Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public MsmqQueue Purge()
        {
            _queue.Purge();
            return this;
        }
    }
}
