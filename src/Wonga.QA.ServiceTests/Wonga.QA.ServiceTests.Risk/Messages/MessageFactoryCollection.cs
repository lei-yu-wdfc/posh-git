using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MessageFactoryCollection
	{
		private readonly IList<IMessageFactory> _messageFactories = new List<IMessageFactory>();



		public void Add<T>(Action<T> init) where T : MessageBase
		{
			_messageFactories.Add(new MessageFactory<T>(init));
		}

		public T Get<T>() where T : MessageBase
		{
			var matches = _messageFactories.OfType<MessageFactory<T>>();
			
			if (matches.Count() == 0) return default(T);
			return matches.First().Message;
		}

		public void ApplyDefaults()
		{
			_messageFactories.ForEach(x => x.ApplyDefaults());
		}

		public void Instantiate()
		{
			_messageFactories.ForEach(x => x.Instantiate());
		}

		public void Initialise()
		{
			_messageFactories.ForEach(x => x.Initialise());
		}

		private IEnumerable<MessageBase> Messages
		{
			get { return _messageFactories.Select(x => x.MsmqMessage); }
		}

		public IEnumerable<MsmqMessage> MsmqMessages 
		{
			get { return Messages.OfType<MsmqMessage>(); }
		}

		public IEnumerable<ApiRequest> ApiRequests
		{
			get { return Messages.OfType<ApiRequest>(); }
		}
	}
}