using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetBankHolidays </summary>
    [XmlRoot("GetBankHolidays")]
    public partial class GetBankHolidaysQuery : ApiRequest<GetBankHolidaysQuery>
    {
    }
}
