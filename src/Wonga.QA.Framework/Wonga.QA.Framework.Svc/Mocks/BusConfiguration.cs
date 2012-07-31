using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace Wonga.QA.Framework.Svc.Mocks
{
	public class BusConfiguration :IConfigurationSource
	{
		public string Endpoint { get; set; }

		public T GetConfiguration<T>() where T : class
		{
			if (typeof(T) == typeof(MsmqTransportConfig))
				return MsmqConfig() as T;

			if (typeof(T) == typeof(UnicastBusConfig))
				return new UnicastBusConfig() as T;

			throw new NotImplementedException(typeof(T).ToString());
		}

		private MsmqTransportConfig MsmqConfig()
		{
			return new MsmqTransportConfig
			{
				ErrorQueue = Endpoint + "_error",
				InputQueue = Endpoint,
				MaxRetries = 5,
				NumberOfWorkerThreads = 1
			};
		}
	}
}
