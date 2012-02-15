
using System;
using Wonga.QA.Framework.Core;
namespace Wonga.QA.Framework.UI.Mappings
{
    internal static class Elements
    {
        internal static BaseElements Get
        {
            get
            {
                switch (Config.AUT)
                {
                    case(AUT.Wb):
                        return new WbElements();
                    case (AUT.Za):
                        return new ZaElements();
                    case (AUT.Ca):
                        return new CaElements();
                }
                throw new NotImplementedException();
            }
        }
    }
}
