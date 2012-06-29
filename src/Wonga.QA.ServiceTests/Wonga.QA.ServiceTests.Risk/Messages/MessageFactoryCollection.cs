using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MessageFactoryCollection
	{
		private readonly IList<IMessageFactory> _messageFactories = new List<IMessageFactory>();



		public void Add<T>(Action<T> init) where T : MsmqMessage
		{
			_messageFactories.Add(new MessageFactory<T>(init));
		}

		public T Get<T>() where T : MsmqMessage
		{
			return _messageFactories.OfType<MessageFactory<T>>().First().Message;
		}

		public void ApplyDefaults()
		{
			_messageFactories.ForEach(x => x.ApplyDefaults());
		}

		public void Initialise()
		{
			_messageFactories.ForEach(x => x.Initialise());
		}

		public static implicit operator MsmqMessage[](MessageFactoryCollection messages)
		{
			return messages._messageFactories.Select(x => x.MsmqMessage).ToArray();
		}
	}
}