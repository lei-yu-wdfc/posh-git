using System;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MessageFactory<T> : IMessageFactory where T : MsmqMessage
	{
		private readonly Action<T> _initialiser;

		public MessageFactory(Action<T> initialiser)
		{
			_initialiser = initialiser;
		}

		public T Message { get; private set; }

		public MsmqMessage MsmqMessage
		{
			get { return Message; }
		}

		private void Instantiate()
		{
			Message = Activator.CreateInstance<T>();
		}

		public void Initialise()
		{
			Instantiate();
			_initialiser(Message);
		}


		public void ApplyDefaults()
		{
			Message.Default();
		}
	}
}