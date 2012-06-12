using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Wonga.QA.Tests.Core
{
    public class SmokeTestAttribute : CategoryAttribute
    {
        public SmokeTestAttribute()
            : base(TestCategories.SmokeTest)
        {
        }
    }
}
