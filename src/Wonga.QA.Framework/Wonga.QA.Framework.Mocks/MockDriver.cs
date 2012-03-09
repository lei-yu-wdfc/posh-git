using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Mocks
{
    public class MockDriver
    {
        /// <summary>
        /// Lazy Scotia object
        /// </summary>
        private Scotia _scotia;

        /// <summary>
        /// Scotia. Instantiated lazily.
        /// </summary>
        public Scotia Scotia
        {
            get { return _scotia ?? (_scotia = new Scotia()); }
        }
    }
}