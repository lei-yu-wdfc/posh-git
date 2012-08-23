using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using MbUnit.Framework;

namespace Wonga.QA.PerformanceTests.Core
{
    public class MsmqOperations
    {
        /// <summary>
        /// Write the given message into the remote queue
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        public void WriteToQueue(string ip, string queueName, Message message)
        {
            var path = "FormatName:Direct=TCP:" + ip + "\\private$\\" + queueName;
            var queue = new MessageQueue(path);

            //Sending to a Transactional queue
            queue.Send(message, MessageQueueTransactionType.Single);
        }

        /// <summary>
        /// Read the message from the remote queue
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public Message ReadFromQueue(string ip, string queueName)
        {
            var path = "FormatName:Direct=TCP:" + ip + "\\private$\\" + queueName;
            var queue = new MessageQueue(path);
            var msg = queue.Receive(MessageQueueTransactionType.Single);

            return msg;
        }

        /// <summary>
        /// Clen up the given Message Queue in the Remote Server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="queueName"></param>
        public void PurgeMessageQueue(string ip, string queueName)
        {
            var path = "FormatName:Direct=TCP:" + ip + "\\private$\\" + queueName;
            var queue = new MessageQueue(path);

            queue.Purge();
        }
    }
}
