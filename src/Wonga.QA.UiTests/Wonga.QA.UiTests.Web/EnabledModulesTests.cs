﻿using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class EnabledModulesTests : UiTest
    {
        // This file contains tests which check the page source to
        // verify that certain modules are enabled, e.g. the Wonga
        // Click module will add a JS tag which has a specific string
        // present on all URLs; the L0 ZA module adds a certain type
        // of insert into L0 journey pages, and so on.
    }
}
