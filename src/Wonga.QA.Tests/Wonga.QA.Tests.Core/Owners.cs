using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.Core
{
    public enum Owner
    {
        [Description("mihail.podobivsky@wonga.com")]
        MihailPodobivsky,
        [Description("volodymyr.stelmakh@wonga.com")]
        VolodymyrStelmakh,
        [Description("kirill.polishyk@wonga.com")]
        KirillPolishyk,
        [Description("petr.tarasenko@wonga.com")]
        PetrTarasenko
    }
}
