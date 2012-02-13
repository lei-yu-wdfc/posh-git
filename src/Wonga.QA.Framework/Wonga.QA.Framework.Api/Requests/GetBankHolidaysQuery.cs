using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBankHolidays")]
    public partial class GetBankHolidaysQuery : ApiRequest<GetBankHolidaysQuery>
    {
    }
}
