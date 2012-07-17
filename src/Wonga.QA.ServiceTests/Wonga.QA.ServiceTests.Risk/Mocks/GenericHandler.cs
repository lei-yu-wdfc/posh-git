using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;

namespace Wonga.QA.ServiceTests.Risk.Mocks
{
	public class GenericHandler : IHandleMessages<IMessage>
	{
		//todo: make instance variable and load generichandler explicitly to the bus
		private static readonly List<OnTheFlyHandler> Handlers = new List<OnTheFlyHandler>();

		public static void Add(OnTheFlyHandler handler)
		{
			Handlers.Add(handler);
		}

		public IBus Bus { get; set; }

		public void Handle(IMessage message)
		{
			var relevantHandlers = Handlers.Where(x => x.IsFor(message)).ToList();
			Console.WriteLine("#{0} Handlers found for this {1}", relevantHandlers.Count, message.GetType().Name);

			relevantHandlers.ForEach(x => x.Handle(message, Bus));
			Handlers.RemoveAll(x => x.IsFor(message));
		}
	}
}
