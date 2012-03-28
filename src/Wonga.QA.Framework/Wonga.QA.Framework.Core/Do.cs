using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Wonga.QA.Framework.Core
{
    public static class Do
    {
        public static TimeSpan Timeout { get { return TimeSpan.FromMinutes(1); } }
        public static TimeSpan Interval { get { return TimeSpan.FromSeconds(2); } }
        public static DoBuilder With { get { return new DoBuilder(Timeout, Interval); } }

        public static T Until<T>(Func<T> predicate)
        {
            return With.Until(predicate);
        }

        public static void While<T>(Func<T> predicate)
        {
            With.While(predicate);
        }

        public static T Watch<T>(Func<T> predicate) where T : struct
        {
            return With.Watch(predicate);
        }
    }

    public class DoBuilder
    {
        private TimeSpan _timeout;
        private TimeSpan _interval;
        private Func<String> _message;

        internal DoBuilder(TimeSpan timeout, TimeSpan interval)
        {
            _timeout = timeout;
            _interval = interval;
        }

        public DoBuilder Timeout(Int32 minutes)
        {
            _timeout = TimeSpan.FromMinutes(minutes);
            return this;
        }

        public DoBuilder Timeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public DoBuilder Interval(Int32 seconds)
        {
            _interval = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public DoBuilder Interval(TimeSpan interval)
        {
            _interval = interval;
            return this;
        }

        public DoBuilder Message(String message, params Object[] arguments)
        {
            _message = () => String.Format(message, arguments);
            return this;
        }

        public DoBuilder Message(Func<String> message)
        {
            _message = message;
            return this;
        }

        public T Until<T>(Func<T> predicate)
        {
            var exception = new DoException(_message);
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < _timeout)
                try
                {
                    var t = predicate();
                    if (!EqualityComparer<T>.Default.Equals(t, default(T)))
                        return t;
                    exception.Add(new Exception(_message == null ? t == null ? "null" : t.ToString() : _message()), stopwatch.Elapsed);
                    Thread.Sleep(_interval);
                }
                catch (Exception e)
                {
                    exception.Add(e, stopwatch.Elapsed);
                    Thread.Sleep(_interval);
                }
            exception.Add(new TimeoutException(stopwatch.Elapsed.ToString()), stopwatch.Elapsed);
            throw exception;
        }

        public void While<T>(Func<T> predicate)
        {
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < _timeout)
                try
                {
                    if (EqualityComparer<T>.Default.Equals(predicate(), default(T)))
                        return;
                    Thread.Sleep(_interval);
                }
                catch
                {
                    return;
                }
            throw new TimeoutException(_message == null ? stopwatch.Elapsed.ToString() : _message());
        }

        public T Watch<T>(Func<T> predicate) where T : struct
        {
            var t = predicate();
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < _timeout)
            {
                Thread.Sleep(_interval);
                var p = predicate();
                if (!EqualityComparer<T>.Default.Equals(p, t))
                    stopwatch.Restart();
                t = p;
            }
            return t;
        }
    }

    public class DoException : Exception
    {
        private Func<String> _message;

        public Tuples Exceptions { get; set; }
        public override String Message { get { return _message == null ? base.Message : _message(); } }

        public DoException(Func<String> message)
        {
            _message = message;
            Exceptions = new Tuples();
        }

        public void Add(Exception exception, TimeSpan span)
        {
            Exceptions.Add(Tuple.Create(DateTime.Now, span, exception));
        }

        public override String ToString()
        {
            return base.ToString() + Environment.NewLine + Exceptions;
        }

        public class Tuples : List<Tuple<DateTime, TimeSpan, Exception>>
        {
            public override String ToString()
            {
                return String.Join(Environment.NewLine, this.Select((t, i) => String.Format("#{0} {1:HH:mm:ss.fff} ({2:0.00000}) : {3}", i, t.Item1, t.Item2.TotalSeconds, t.Item3))) + Environment.NewLine;
            }
        }
    }
}
