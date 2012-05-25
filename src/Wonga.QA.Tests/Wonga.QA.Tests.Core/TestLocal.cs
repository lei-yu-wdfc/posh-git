using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Gallio.Framework;

namespace Wonga.QA.Tests.Core
{
    public class TestLocal<T> : ITestLocal<T>
    {
        private Dictionary<string, T> _dict = new Dictionary<string,T>();
        private Func<T> _function;
        private static object _lock = new object();

        public T Value
        {
            get
            {
                string testKey = null;
                lock(_lock)
                {
                    testKey = TestContext.CurrentContext.TestStep.Id;
                    if (!_dict.ContainsKey(testKey))
                    {
                        Trace.WriteLine("New test: " + testKey);
                        _dict[testKey] = _function();
                    }
                }
                return _dict[testKey];
            }
        }

        public TestLocal(Func<T> function)
        {
            _function = function;
        }
    }

    public interface ITestLocal<T>
    {
        T Value { get;}
    }
}
