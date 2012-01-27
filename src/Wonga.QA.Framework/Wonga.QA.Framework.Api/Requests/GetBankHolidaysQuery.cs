using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBankHolidays")]
    public class GetBankHolidaysQuery : ApiRequest<GetBankHolidaysQuery>
    {
    }
}
