using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    public class RepaymentDetailsDescriptor
    {
        [XmlElement(DataType = "date")]
        public DateTime RepaymentDate { get; set; }

        public decimal RepaymentAmount { get; set; }
    }
}
