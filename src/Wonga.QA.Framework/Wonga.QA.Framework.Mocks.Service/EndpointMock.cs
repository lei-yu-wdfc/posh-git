using System;
using NServiceBus;
using Wonga.QA.Framework.Msmq;


namespace Wonga.QA.Framework.Mocks.Service
{
	public class EndpointMock:IDisposable
	{
		private readonly string _name;
		private readonly QafMessageMapper _mapper = new QafMessageMapper();
		private  IDisposable _bus;
		private MsmqQueue _queueUnderTest;


		public EndpointMock(string endpointName,MsmqQueue queueUnderTest)
		{
			_name = endpointName;
			_queueUnderTest = queueUnderTest;
		}

		public void Start()
		{
			_bus = new EndpointConfigurator(_name).InitialiseEndpoint();
		}

		public void AddHandler<T>(Action<T> action) where T : IMessage
		{
			AddHandler(null, action);
		}

		public OnTheFlyHandler<T> AddHandler<T>(Func<T, bool> filter, Action<T> action) where T : IMessage
		{
			var handler = new OnTheFlyHandler<T>(filter, action);
			GenericHandler.Add(handler);
			return handler;
		}

		public void Subscribe<T>() where T : IMessage
		{
			
			_queueUnderTest.SendSubscription(SubscriptionTo<T>(),  _name + "@localhost");
		}

		private string SubscriptionTo<T>() where T:IMessage
		{
			return
				string.Format(
					"<?xml version=\"1.0\"?><string>{0}, {1}</string>",
					_mapper.GetSourceTypeName(typeof(T).FullName),
					_mapper.GetSourceAssemblyName(typeof(T)));
		}

		#region Disposable pattern

		private bool IsDisposed { get; set; }

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			try
			{
				if (!IsDisposed
					&& isDisposing
					&& _bus != null)
				{
					_bus.Dispose();
					_bus = null;
				}
			}
			finally
			{
				IsDisposed = true;
			}
		}

		#endregion

	}
}
	