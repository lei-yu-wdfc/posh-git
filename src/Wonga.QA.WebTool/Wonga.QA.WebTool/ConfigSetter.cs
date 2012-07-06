using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.WebTool
{
    public class ConfigSetter
    {
        public void Setter(string aut, string sut)
        {
            switch (aut)
            {
                case "Za": Config.AUT = Framework.Core.AUT.Za;
                    break;
                case "Ca": Config.AUT = Framework.Core.AUT.Ca;
                    break;
                case "Wb": Config.AUT = Framework.Core.AUT.Wb;
                    break;
                case "Uk": Config.AUT = Framework.Core.AUT.Uk;
                    break;
                case "Pl": Config.AUT = Framework.Core.AUT.Pl;
                    break;
            }

            switch (sut)
            {
                case "RC":
                    Config.SUT = Framework.Core.SUT.RC;
                    Config.Api = new Config.ApiConfig(String.Format("rc.api.{0}.wonga.com", Config.AUT));
                    Config.Cs = new Config.CsConfig(String.Format("rc.csapi.{0}.wonga.com", Config.AUT));
                    Config.Svc = Config.AUT == Framework.Core.AUT.Uk ? new Config.SvcConfig("RC2") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.SvcConfig("RC4") :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.SvcConfig("RC6") :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.SvcConfig("RC9", "RC10") :
                        Config.Throw<Config.SvcConfig>();
                    Config.Msmq = Config.AUT == Framework.Core.AUT.Uk ? new Config.MsmqConfig("RC2") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.MsmqConfig("RC4") :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.MsmqConfig("RC6") :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.MsmqConfig("RC9", "RC10") :
                        Config.Throw<Config.MsmqConfig>();
                    Config.Db = Config.AUT == Framework.Core.AUT.Uk ? new Config.DbConfig(Connections.GetDbConn("RC2", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.DbConfig(Connections.GetDbConn("RC4", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.DbConfig(Connections.GetDbConn("RC6", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.DbConfig(Connections.GetDbConn("RC8", Config.Proxy)) :
                        Config.Throw<Config.DbConfig>();
                    break;
                case "WIPRelase":
                    Config.SUT = Framework.Core.SUT.WIPRelease;
                    Config.Api = new Config.ApiConfig(String.Format("wip.release.api.{0}.wonga.com", Config.AUT));
                    Config.Cs = new Config.CsConfig(String.Format("wip.release.csapi.{0}.wonga.com", Config.AUT));
                    Config.Svc =
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.SvcConfig("ca-rel-wip-app") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.SvcConfig("za-rel-wip-app") : Config.Throw<Config.SvcConfig>();
                    Config.Msmq =
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.MsmqConfig("ca-rel-wip-app") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.MsmqConfig("za-rel-wip-app") : Config.Throw<Config.MsmqConfig>();
                    Config.Db =
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.DbConfig(Connections.GetDbConn("ca-rel-wip-app", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.DbConfig(Connections.GetDbConn("za-rel-wip-app", Config.Proxy)) : Config.Throw<Config.DbConfig>();
                    break;
                case "RCRelease":
                    Config.SUT = Framework.Core.SUT.RCRelease;
                    Config.Api = new Config.ApiConfig(String.Format("rc.release.api.{0}.wonga.com", Config.AUT));
                    Config.Cs = new Config.CsConfig(String.Format("rc.release.csapi.{0}.wonga.com", Config.AUT));
                    Config.Svc =
                         Config.AUT == Framework.Core.AUT.Ca ? new Config.SvcConfig("ca-rel-rc-app") :
                         Config.AUT == Framework.Core.AUT.Za ? new Config.SvcConfig("za-rel-rc-app") : Config.Throw<Config.SvcConfig>();
                    Config.Msmq =
                         Config.AUT == Framework.Core.AUT.Ca ? new Config.MsmqConfig("ca-rel-rc-app") :
                         Config.AUT == Framework.Core.AUT.Za ? new Config.MsmqConfig("za-rel-rc-app") : Config.Throw<Config.MsmqConfig>();
                    Config.Db =
                         Config.AUT == Framework.Core.AUT.Ca ? new Config.DbConfig(Connections.GetDbConn("ca-rel-rc-app", Config.Proxy)) :
                         Config.AUT == Framework.Core.AUT.Za ? new Config.DbConfig(Connections.GetDbConn("za-rel-rc-app", Config.Proxy)) : Config.Throw<Config.DbConfig>();
                    break;
                case "WIP":
                    Config.SUT = Framework.Core.SUT.WIP;
                    Config.Api = new Config.ApiConfig(String.Format("wip.api.{0}.wonga.com", Config.AUT));
                    Config.Cs = new Config.CsConfig(String.Format("wip.csapi.{0}.wonga.com", Config.AUT));
                    Config.Svc =
                        Config.AUT == Framework.Core.AUT.Uk ? new Config.SvcConfig("WIP2") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.SvcConfig("WIP4") :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.SvcConfig("WIP6") :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.SvcConfig("WIP8") : Config.Throw<Config.SvcConfig>();
                    Config.Msmq =     
                        Config.AUT == Framework.Core.AUT.Uk ? new Config.MsmqConfig("WIP2") :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.MsmqConfig("WIP4") :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.MsmqConfig("WIP6") :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.MsmqConfig("WIP8") : Config.Throw<Config.MsmqConfig>();
                    Config.Db =       
                        Config.AUT == Framework.Core.AUT.Uk ? new Config.DbConfig(Connections.GetDbConn("WIP2", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Za ? new Config.DbConfig(Connections.GetDbConn("WIP4", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Ca ? new Config.DbConfig(Connections.GetDbConn("WIP6", Config.Proxy)) :
                        Config.AUT == Framework.Core.AUT.Wb ? new Config.DbConfig(Connections.GetDbConn("WIP8", Config.Proxy)) : Config.Throw<Config.DbConfig>();
                    break;
            }
        }
    }
}