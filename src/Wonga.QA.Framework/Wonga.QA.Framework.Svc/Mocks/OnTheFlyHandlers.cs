using System;
using NServiceBus;

namespace Wonga.QA.Framework.Svc.Mocks
{
	public class OnTheFlyHandler
	{
		public virtual bool IsFor(IMessage msg) { return true; }
		public virtual void Handle(IMessage msg) { }
	}

	public class OnTheFlyHandler<T> : OnTheFlyHandler where T : IMessage
	{
		private readonly Func<T, bool> _filter;
		private readonly Action<T> _action;

		public OnTheFlyHandler(Func<T, bool> filter, Action<T> action)
		{
			_filter = filter;
			_action = action;
		}
		public OnTheFlyHandler(Action<T> action) : this(null, action) { }

		public override bool IsFor(IMessage msg)
		{
			return base.IsFor(msg) &&
						msg is T &&
					(_filter == null || _filter((T)msg));
		}

		public override void Handle(IMessage msg)
		{
			base.Handle(msg);
			StronglyTypedHandle((T)msg);
		}

		public void StronglyTypedHandle(T handle)
		{
			_action(handle);
		}
	}
}
