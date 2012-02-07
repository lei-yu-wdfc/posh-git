using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBanks")]
    public partial class GetBanksQuery : ApiRequest<GetBanksQuery>
    {
    }
}
