using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Wonga.QA.Framework.Core
{
    public static class Do
    {
        public static TimeSpan Timeout { get { return TimeSpan.FromSeconds(60); } }
        public static TimeSpan Interval { get { return TimeSpan.FromSeconds(1); } }

        public static DoBuilder With()
        {
            return new DoBuilder(Timeout, Interval);
        }

        public static T Until<T>(Func<T> predicate)
        {
            return With().Until(predicate);
        }

        public static void While<T>(Func<T> predicate)
        {
            With().While(predicate);
        }

        public static T Watch<T>(Func<T> predicate) where T : struct
        {
            return With().Watch(predicate);
        }
    }

    public class DoBuilder
    {
        private TimeSpan _timeout;
        private TimeSpan _interval;
        private Func<String> _message;

        public DoBuilder(TimeSpan timeout, TimeSpan interval)
        {
            _timeout = timeout;
            _interval = interval;
            _message = () => null;
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
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < _timeout)
                try
                {
                    T t = predicate();
                    if (!EqualityComparer<T>.Default.Equals(t, default(T)))
                        return t;
                    Thread.Sleep(_interval);
                }
                catch
                {
                    Thread.Sleep(_interval);
                }
            throw new TimeoutException(_message());
        }

        public void While<T>(Func<T> predicate)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
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
            throw new TimeoutException(_message());
        }

        public T Watch<T>(Func<T> predicate) where T : struct
        {
            T t = predicate();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < _timeout)
            {
                Thread.Sleep(_interval);
                T p = predicate();
                if (!EqualityComparer<T>.Default.Equals(p, t))
                    stopwatch.Restart();
                t = p;
            }
            return t;
        }
    }
}
