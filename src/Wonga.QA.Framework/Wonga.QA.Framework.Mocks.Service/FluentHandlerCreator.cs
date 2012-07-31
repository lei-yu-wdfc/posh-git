using System;
using NServiceBus;

namespace Wonga.QA.Framework.Mocks.Service
{
	public class FluentHandlerCreator<T> where T:IMessage
	{
		private  Func<T, bool> _filter;
		private readonly EndpointMock _endpoint;

		public FluentHandlerCreator(EndpointMock endpoint)
		{
			_endpoint = endpoint;
		}
		
		public FluentHandlerCreator<T> Matching(Func<T, bool> filter)
		{
			_filter = filter;
			return this;
		}

		public FluentHandlerCreator<T> ThenDoThis(Action<T> action)
		{
			_endpoint.AddHandler(_filter, action);
			return this;
		}

		public FluentHandlerCreator<T> SeemsLegit()
		{
			return this;
		}
		public void Dude() { }
	}

	public static class FluentEndpointMock
	{
		public static FluentHandlerCreator<T> OnArrivalOf<T>(this EndpointMock endpointMock) where T: IMessage
		{
			return new FluentHandlerCreator<T>(endpointMock);
		}

		public static FluentHandlerCreator<T> SubscribeTo<T>(this EndpointMock endpointMock) where T : IMessage
		{
			endpointMock.Subscribe<T>();
			return new FluentHandlerCreator<T>(endpointMock);
		}
	}
}
