using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveEducationalBackgroundDetails </summary>
    [XmlRoot("SaveEducationalBackgroundDetails")]
    public partial class SaveEducationalBackgroundDetailsPlCommand : ApiRequest<SaveEducationalBackgroundDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object EducationalLevel { get; set; }
    }
}
