using System;
using NServiceBus;

namespace Wonga.QA.ServiceTests.Risk.Mocks
{
	public class EndpointMock
	{
		private readonly string _name;

		public EndpointMock(string endpointName)
		{
			_name = endpointName;
		}

		public void Start()
		{
			var ep1 = new EndpointConfigurator(_name);
			ep1.InitialiseEndpoint();
		}

		public void AddHandler<T>(Action<T, IBus> action) where T : IMessage
		{
			AddHandler(null, action);
		}

		public void AddHandler<T>(Func<T, bool> filter, Action<T, IBus> action) where T : IMessage
		{
			GenericHandler.Add(new OnTheFlyHandler<T>(filter, action));
		}

	}
}
