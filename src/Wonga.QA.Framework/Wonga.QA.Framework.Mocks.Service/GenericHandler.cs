using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;

namespace Wonga.QA.Framework.Mocks.Service
{
	public class GenericHandler : IHandleMessages<IMessage>
	{
		public IBus Bus { get; set; }
		private static readonly List<OnTheFlyHandler> Handlers = new List<OnTheFlyHandler>();

		public static void Add(OnTheFlyHandler handler)
		{
			Handlers.Add(handler);
		}
		
		public void Handle(IMessage message)
		{
			FindHandlersFor(message)
				.ForEach(x => x.Handle(message));

			RemoveHandlersFor(message);
		}

		private List<OnTheFlyHandler> FindHandlersFor(IMessage message)
		{
			var relevantHandlers = Handlers.Where(x => x.IsFor(message)).ToList();
			Console.WriteLine("#{0} Handlers found for this {1}", relevantHandlers.Count, message.GetType().Name);

			return relevantHandlers;
		}

		private void RemoveHandlersFor(IMessage message)
		{
			Handlers.RemoveAll(x => x.IsFor(message));
		}
	}
}
