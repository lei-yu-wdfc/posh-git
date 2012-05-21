using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.Core
{
    /// <summary>
    /// Use the TestLayer in order to store unique ordering values that can be used so TestFixtures get
    /// grouped and parallelized when that's needed.
    /// You can use this layering in the Test attribute as follows:
    /// 
    /// Test(Order = TestLayer.DummyFixtures)
    /// public class DummyFixturesClass1
    /// </summary>
    public class TestLayer
    {
        public const int DummyFixtures = 0;
    }
}
