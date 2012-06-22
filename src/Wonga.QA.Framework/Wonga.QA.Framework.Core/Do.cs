using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Wonga.QA.Framework.Core
{
    public static class Do
    {
        public static TimeSpan Timeout { get { return TimeSpan.FromSeconds(90); } }
        public static TimeSpan Interval { get { return TimeSpan.FromSeconds(2); } }
        public static DoBuilder With { get { return new DoBuilder(Timeout, Interval); } }
        public static DoBuilder WhenAutIn(AUT[] aut)
        {
            var doBuilder = new DoBuilder(Timeout, Interval);
            doBuilder.WhenAutIsIn(aut);
            return doBuilder;
        }

        public static DoBuilder WhenAutNotIn(AUT[] aut)
        {
            var doBuilder = new DoBuilder(Timeout, Interval);
            doBuilder.WhenAutIsNotIn(aut);
            return doBuilder;
        }

        public static DoBuilder WhenAutIs(AUT aut)
        {
            var doBuilder = new DoBuilder(Timeout, Interval);
            doBuilder.WhenAutIsIn(new[] { aut });
            return doBuilder;
        }

        public static DoBuilder WhenAutIsNot(AUT aut)
        {
            var doBuilder = new DoBuilder(Timeout, Interval);
            doBuilder.WhenAutIsNotIn(new[] { aut });
            return doBuilder;
        }
        /// <summary>
        /// Until will take your function and execute it until the result is not equal to it's default value.
        /// eg bool return should return true, objects should return something other than null etc. If this does not
        /// happen for the Timeout duration it will throw an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T Until<T>(Func<T> predicate)
        {
            return With.Until(predicate);
        }

        /// <summary>
        /// While will try to execute your function per Interval until your function returns 
        /// the default value of it's returning type. eg false for boolean, null for objects etc.
        /// In the case that the default value is not returned after the timeout has elapsed it will
        /// throw an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public static void While<T>(Func<T> predicate)
        {
            With.While(predicate);
        }

        /// <summary>
        /// This method will wait until the returned object of your function has a constant value
        /// for duration you set on the Timeout property.
        /// Please note that if your value keeps changing before the Timeout has elapsed
        /// then it will run indefinetly. Each time the return value of your function changes
        /// then the time count resets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">A function with return type T</param>
        /// <returns>T</returns>
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
        private Func<bool> _appSettingsFunction;

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

        /// <summary>
        /// Until will take your function and execute it until the result is not equal to it's default value.
        /// eg bool return should return true, objects should return something other than null etc. If this does not
        /// happen for the Timeout duration it will throw an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Until<T>(Func<T> predicate)
        {
            if (!DoRunInThisAut())
                return default(T);
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

        /// <summary>
        /// While will try to execute your function per Interval until your function returns 
        /// the default value of it's returning type. eg false for boolean, null for objects etc.
        /// In the case that the default value is not returned after the timeout has elapsed it will
        /// throw an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void While<T>(Func<T> predicate)
        {
            if (!DoRunInThisAut())
                return;
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

        /// <summary>
        /// This method will wait until the returned object of your function has a constant value
        /// for duration you set on the Timeout property.
        /// Please note that if your value keeps changing before the Timeout has elapsed
        /// then it will run indefinetly. Each time the return value of your function changes
        /// then the time count resets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">A function with return type T</param>
        /// <returns>T</returns>
        public T Watch<T>(Func<T> predicate) where T : struct
        {
            if (!DoRunInThisAut())
                return default(T);
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

        /// <summary>
        /// Executes your function immediately with no retries.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Execute<T>(Func<T> predicate)
        {
            if (!DoRunInThisAut())
                return default(T);
            return predicate();
        }

        private bool DoRunInThisAut()
        {
            if (_appSettingsFunction == null)
                return true;
            return _appSettingsFunction.Invoke();
        }

        public DoBuilder WhenAutIsIn(AUT[] auts)
        {
            _appSettingsFunction = delegate { return auts.Any(x => x == Config.AUT); };
            return this;
        }

        public DoBuilder WhenAutIsNotIn(AUT[] auts)
        {
            _appSettingsFunction = delegate { return !auts.Any(x => x == Config.AUT); };
            return this;
        }

        public DoBuilder WhenAutIs(AUT aut)
        {
            _appSettingsFunction = delegate { return aut == Config.AUT; };
            return this;
        }

        public DoBuilder WhenAutIsNot(AUT aut)
        {
            _appSettingsFunction = delegate { return aut != Config.AUT; };
            return this;
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
