using System;
using System.Transactions;
using Autofac;
using NServiceBus;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Serialization;
using NServiceBus.Serializers.XML;
using NServiceBus.Unicast.Transport;

namespace Wonga.QA.Framework.Mocks.Service
{
	internal class EndpointConfigurator
	{
		private string EndpointName { get; set; }

		public IDisposable InitialiseEndpoint()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<BusConfiguration>().As<IConfigurationSource>().PropertiesAutowired().
				InstancePerLifetimeScope();
			var container = builder.Build();

			var busConfigSource = container.Resolve<IConfigurationSource>() as BusConfiguration;
			busConfigSource.Endpoint = EndpointName;
			var configure = Configure.With(
				typeof (IMessage).Assembly,
				typeof (CompletionMessage).Assembly,
				typeof (OnTheFlyHandler).Assembly,
				typeof (Framework.Msmq.Messages.Risk.IRiskEvent).Assembly
				);

			configure.CustomConfigurationSource(busConfigSource);
			configure.Autofac2Builder(container).XmlSerializer();
			var bus = configure.MsmqTransport()
				.IsTransactional(true)
				.IsolationLevel(IsolationLevel.RepeatableRead)
				.PurgeOnStartup(false)
				.UnicastBus()
				.ImpersonateSender(true)
				.LoadMessageHandlers()
				.DoNotAutoSubscribe().CreateBus();
			bus.Start();

			var messageSerializer = container.Resolve<IMessageSerializer>();
			var origMapper = ((MessageSerializer) messageSerializer).MessageMapper;
			((MessageSerializer) messageSerializer).MessageMapper = new QafMessageMapper {Decorated = origMapper};

			return bus;
		}

		public EndpointConfigurator(string endpointName)
		{
			EndpointName = endpointName;
		}
		
	}
}
