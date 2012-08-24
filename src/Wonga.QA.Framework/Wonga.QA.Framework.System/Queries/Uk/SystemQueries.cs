namespace Wonga.QA.Framework.System.Queries.Uk
{
	public sealed class SystemQueries : SystemQueriesBase
	{
		public PayLaterSystemQueries PayLater { get; private set; }

		public SystemQueries()
		{
			PayLater = new PayLaterSystemQueries();
		}
	}
}
