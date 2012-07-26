using System;
using NServiceBus.MessageInterfaces;
using Wonga.QA.Framework.Msmq.Messages.Risk;

namespace Wonga.QA.ServiceTests.Risk.Mocks
{
	public class QafMessageMapper : IMessageMapper
	{

		public IMessageMapper Decorated { get; set; }

		public Type GetMappedTypeFor(string typeName)
		{
			if (!IsWongaMessage(typeName))
				return Decorated.GetMappedTypeFor(typeName);

			return MapToGeneratedType(typeName);
		}

		public Type GetMappedTypeFor(Type t)
		{
			return Decorated.GetMappedTypeFor(t);
		}

		public void Initialize(System.Collections.Generic.IEnumerable<Type> types) { }

		public object CreateInstance(Type messageType)
		{
			return Decorated.CreateInstance(messageType);
		}

		public T CreateInstance<T>(Action<T> action) where T : NServiceBus.IMessage
		{
			throw new NotImplementedException();
		}

		public T CreateInstance<T>() where T : NServiceBus.IMessage
		{
			throw new NotImplementedException();
		}

		private bool IsWongaMessage(string typeName)
		{
			return typeName.ToLower().Contains("wonga");
		}

		private string GetMappedName(string typeName)
		{
			return typeName.Replace("Wonga", "Wonga.QA.Framework.Msmq.Messages");
		}

		private Type MapToGeneratedType(string typeName)
		{
			var mapped = LoadType(GetMappedName(typeName));
			if (mapped == null)
				throw new TypeLoadException("Could not map message type " + typeName);
			return Decorated.GetMappedTypeFor(mapped);
		}

		private Type LoadType(string type)
		{
			return typeof (IApplicationAccepted).Assembly.GetType(type);
		}

	}
	
}
