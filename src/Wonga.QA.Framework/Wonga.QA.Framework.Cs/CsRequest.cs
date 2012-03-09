using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Cs
{
    public abstract class CsRequest : ApiRequest { }
    public abstract class CsRequest<T> : CsRequest where T : CsRequest<T>, new() { }
}
