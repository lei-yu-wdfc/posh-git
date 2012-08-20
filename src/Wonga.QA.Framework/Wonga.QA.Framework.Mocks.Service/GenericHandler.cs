using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using Wonga.QA.ServiceTests.Risk.Mocks;
using log4net;

namespace Wonga.QA.Framework.Mocks.Service
{
	public class GenericHandler : IHandleMessages<IMessage>
	{
        public IBus Bus { get; set; }
        public static ILog Logger = LogManager.GetLogger("Wonga.QA.Framework.Mocks.Service");

        private static readonly List<OnTheFlyHandler> Handlers = new List<OnTheFlyHandler>();

        public static void Add(OnTheFlyHandler handler)
        {
            Handlers.Add(handler);
        }

        public void Handle(IMessage message)
        {
            FindHandlersFor(message)
                .ForEach(x => x.Handle(message, Bus));

            RemoveHandlersFor(message);

            Logger.Info("==========================================================================");
            Logger.InfoFormat("Received request {0}.", "asd");
            Logger.InfoFormat("String received: {0}.", "asd");
            Logger.InfoFormat("Header 'Test' = {0}.", "asd");
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
