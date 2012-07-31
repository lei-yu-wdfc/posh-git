using System;
using System.Collections.Generic;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using MbUnit.Framework;

namespace Wonga.QA.DataTests.Hds.Common
{
    public abstract class HdsUtilitiesBase
    {
        #region "Constructors"

        protected HdsUtilitiesBase(WongaService wongaService)
        {
            WongaServiceUsed = wongaService;
        }

        protected HdsUtilitiesBase()
        {
            throw new NotImplementedException();
        }

        #endregion "Constructors"

        #region "Properties"

        public enum SystemComponent
        {
            CDCStaging = 1,
            HDS = 2
        }

        public enum WongaService
        {
            Ops = 1,
            Payments = 2,
            Comms = 3,
            Risk = 4
        }

        private WongaService WongaServiceUsed { get; set; }

        public String WongaServiceName
        {
            get { return this.WongaServiceUsed.ToString(); }
        }

        public string WongaServiceSchema
        {
            get
            {
                return (WongaServiceUsed == WongaService.Payments
                            ? WongaServiceName.Substring(0, WongaServiceName.Length - 1)
                            : WongaServiceName).ToLower();
            }
        }

        /// <summary>
        /// Define or retrieve the Region
        /// This will need to change when we have different set ups (like WB for ZA)
        /// </summary>
        public string Region { get { return Config.AUT == AUT.Wb ? "Uk" : Config.AUT.ToString(); } }

        /// <summary>
        /// Define or retreive the Product
        /// </summary>
        public string Product { get { return Config.AUT == AUT.Wb ? Config.AUT.ToString() : ""; } }

        public string CDCDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "CDCStaging";
            }
        }

        public string HDSDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "WongaHDS";
            }
        }

        #endregion "Properties"
    }
}