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
                lock(_lock)
                {
                    if (!_dict.ContainsKey(GetTestId()) && _function != null)
                        _dict[GetTestId()] = _function();
                    return _dict.ContainsKey(GetTestId()) ? _dict[GetTestId()] : default(T);
                }
            }
            set { _dict[GetTestId()] = value; }
        }

        private string GetTestId()
        {
            return TestContext.CurrentContext.TestStep.Id;
        }

        public TestLocal(Func<T> function = null)
        {
            _function = function;
        }
    }

    public interface ITestLocal<T>
    {
        T Value { get;}
    }
}
