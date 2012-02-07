using System;
using System.Messaging;
using System.Text;

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

        public MsmqQueue Send(MsmqMessage message)
        {
            Byte[] bytes = Encoding.Default.GetBytes(message.ToString());
            Message o = new Message();
            o.BodyStream.Write(bytes, 0, bytes.Length);
            _queue.Send(o, MessageQueueTransactionType.Single);
            message.Id = o.Id;
            return this;
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
