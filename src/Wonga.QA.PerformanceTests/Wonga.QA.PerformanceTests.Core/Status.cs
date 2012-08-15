using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Wonga.QA.PerformanceTests.Core
{
    public sealed class Status
    {
        public static readonly Status Running = new Status(0);
        public static readonly Status Completed = new Status(1);
        public static readonly Status Aborted = new Status(2);
        public static readonly Status Error = new Status(3);

        private Status(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }

        /// <summary>
        /// Override method to return the String equivalent of the Integer
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            switch (Value)
            {
                case 0:
                    return "Running";
                case 1:
                    return "Completed";
                case 2:
                    return "Aborted";
                case 3:
                    return "Error";
                default:
                    return "Unknown Status";
            }
        }
    }
}
