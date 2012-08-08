using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;

namespace Wonga.QA.PerformanceTests.Load
{
	public class LoadRunner
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(LoadRunner));

		private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);

		public LoadRunner()
		{
			Duration = TimeSpan.FromSeconds(10);
			Concurrency = 10;
		}

		public Action Test { get; set; }

		public TimeSpan ThinkTime { get; set; }

		public TimeSpan Duration { get; set; }
		
		public int Concurrency { get; set; }
		
		public string TestName { get; set; }

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Run()
		{
			if (string.IsNullOrEmpty(TestName))
			{
				var stackTrace = new StackTrace();
				TestName = stackTrace.GetFrame(1).GetMethod().Name;
			}

			var list = StartConcurrent(Concurrency).ToList();

			Thread.Sleep((int)Duration.TotalMilliseconds);

			list.ForEach(t => t.Abort());
			list.ForEach(t => t.Join());
		}

		private IEnumerable<Thread> StartConcurrent(int concurrent)
		{
			for (int i = 0; i < concurrent; i++)
			{
				int i1 = i;
				var thread = new Thread(t =>
				{
					var offset = DateTime.Now;
					var watch = new Stopwatch();
					var rnd = new Random(Environment.TickCount + i1);
					watch.Start();

					while (true)
					{
						watch.Reset();

						try
						{
							Test();
						}
						catch (Exception exception)
						{
							_log.Error(exception);
						}

						_log.InfoFormat("{0}\t{1}\t{2}", TestName, DateTime.Now - offset, watch.ElapsedMilliseconds);

						if (_stopEvent.WaitOne(0)) return;

						Thread.Sleep((int)(ThinkTime.TotalMilliseconds * rnd.NextDouble()));
					}
				});
				thread.Start();

				yield return thread;
			}
		}
	}
}
