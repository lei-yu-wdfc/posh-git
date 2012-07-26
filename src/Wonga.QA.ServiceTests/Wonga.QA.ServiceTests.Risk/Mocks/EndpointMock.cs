using System;
using NServiceBus;

namespace Wonga.QA.ServiceTests.Risk.Mocks
{
	public class EndpointMock
	{
		private readonly string _name;
		private IBus _bus;

		public EndpointMock(string endpointName)
		{
			_name = endpointName;
		}

		public void Start()
		{
			var ep1 = new EndpointConfigurator(_name);
			_bus = ep1.InitialiseEndpoint();
		}

		public void AddHandler<T>(Action<T, IBus> action) where T : IMessage
		{
			AddHandler(null, action);
		}

		public void AddHandler<T>(Func<T, bool> filter, Action<T, IBus> action) where T : IMessage
		{
			GenericHandler.Add(new OnTheFlyHandler<T>(filter, action));
		}

		public void Subscribe<T>(Predicate<T> condition) where T : IMessage
		{
			_bus.Subscribe<T>(condition);
		}


	}
}
