using System;
using System.Diagnostics;
using System.Threading;

namespace Wonga.QA.Framework.Core
{
    public static class Do
    {
        private static TimeSpan _timeout = TimeSpan.FromSeconds(30);
        private static TimeSpan _interval = TimeSpan.FromSeconds(1);

        public static void Sleep()
        {
            Thread.Sleep(_timeout);
        }

        public static void Sleep(Int32 seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        public static void Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        public static T Until<T>(Func<T> func)
        {
            return Until(func, _timeout, _interval);
        }

        public static T Until<T>(Func<T> func, TimeSpan timeout)
        {
            return Until(func, timeout, _interval);
        }

        public static T Until<T>(Func<T> func, TimeSpan timeout, TimeSpan interval)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < timeout)
                try
                {
                    T t = func();
                    Trace.WriteLine(t, typeof(Do).FullName);
                    if (!Equals(t, default(T)))
                        return t;
                    Sleep(interval);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e, typeof(Do).FullName);
                    Sleep(interval);
                }
            throw new TimeoutException();
        }

        public static void While<T>(Func<T> func)
        {
            While(func, _timeout, _interval);
        }

        public static void While<T>(Func<T> func, TimeSpan timeout)
        {
            While(func, timeout, _interval);
        }

        public static void While<T>(Func<T> func, TimeSpan timeout, TimeSpan interval)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < timeout)
                try
                {
                    T t = func();
                    Trace.WriteLine(t, typeof(Do).FullName);
                    if (Equals(t, default(T)))
                        return;
                    Sleep(interval);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e, typeof(Do).FullName);
                    return;
                }
            throw new TimeoutException();
        }
    }
}
