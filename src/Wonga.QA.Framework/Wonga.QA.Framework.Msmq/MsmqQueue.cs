using System;
using System.Messaging;
using System.Text;

namespace Wonga.QA.Framework.Msmq
{
    public class MsmqQueue
    {
        private MessageQueue _queue;
        private MessageQueue _journal;
        private MessageQueue _error;
        private MessageQueue _subscription;

        public MsmqQueue(String path)
        {
            _queue = new MessageQueue(path);
            _journal = new MessageQueue(String.Format("{0}\\journal$", path));
            _error = new MessageQueue(String.Format("{0}_error", path));
            _subscription = new MessageQueue(String.Format("{0}_subscription", path));
        }

        public void Send(MsmqMessage message)
        {
            Byte[] bytes = Encoding.Default.GetBytes(message.ToString());
            Message o = new Message();
            o.BodyStream.Write(bytes, 0, bytes.Length);
            _queue.Send(o, MessageQueueTransactionType.Single);
        }

        public void Wait(MsmqMessage message)
        {
            throw new NotImplementedException();
        }

        public MsmqMessage Find(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
