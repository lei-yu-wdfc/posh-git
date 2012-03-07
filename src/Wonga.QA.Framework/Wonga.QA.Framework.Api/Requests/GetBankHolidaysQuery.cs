using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetBankHolidays </summary>
    [XmlRoot("GetBankHolidays")]
    public partial class GetBankHolidaysQuery : ApiRequest<GetBankHolidaysQuery>
    {
    }
}
