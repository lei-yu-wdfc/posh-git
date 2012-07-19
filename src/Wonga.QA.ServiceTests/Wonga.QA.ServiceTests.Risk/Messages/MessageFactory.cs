using System;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MessageFactory<T> : IMessageFactory where T : MessageBase
	{
		private readonly Action<T> _initialiser;

		public MessageFactory(Action<T> initialiser)
		{
			_initialiser = initialiser;
		}

		public T Message { get; private set; }

		public MessageBase MsmqMessage
		{
			get { return Message; }
		}

		public void Instantiate()
		{
			Message = Activator.CreateInstance<T>();
		}

		public void Initialise()
		{
			_initialiser(Message);
		}


		public void ApplyDefaults()
		{
			Message.Default();
		}
	}
}