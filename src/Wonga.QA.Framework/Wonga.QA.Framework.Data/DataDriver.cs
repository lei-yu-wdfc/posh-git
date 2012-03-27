using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;
using Simple.Data.Ado;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Data
{
    public class DataDriver
    {
        private Database _ops;
        private Database _opsLogs;
        private Database _comms;
        private Database _payments;
        private Database _risk;
        private Database _bi;
        private Database _bankGateway;
        private Database _blacklist;
        private Database _callReport;
        private Database _callValidate;
        private Database _cardPayment;
        private Database _coldStorage;
        private Database _contactManagement;
        private Database _experian;
        private Database _experianBulk;
        private Database _fileStorage;
        private Database _hpi;
        private Database _ipLookup;
        private Database _qaData;
        private Database _salesforce;
        private Database _scheduler;
        private Database _sms;
        private Database _timeZone;
        private Database _transUnion;
        private Database _uru;
        private Database _wongaPay;
        private Database _opsSagas;

        public dynamic OpsSagas
        {
            get { return _opsSagas ?? (_opsSagas = Database.OpenConnection(Config.Db.OpsSagas));}
            set { _opsSagas = value; }
        }
        
        public dynamic Ops
        {
            get { return _ops ?? (_ops = Database.OpenConnection(Config.Db.Ops)); }
            set { _ops = value; }
        }

        public dynamic OpsLogs
        {
            get { return _opsLogs ?? (_opsLogs = Database.OpenConnection(Config.Db.OpsLogs)); }
            set { _opsLogs = value; }
        }

        public dynamic Comms
        {
            get { return _comms ?? (_comms = Database.OpenConnection(Config.Db.Comms)); }
            set { _comms = value; }
        }

        public dynamic Payments
        {
            get { return _payments ?? (_payments = Database.OpenConnection(Config.Db.Payments)); }
            set { _payments = value; }
        }

        public dynamic Risk
        {
            get { return _risk ?? (_risk = Database.OpenConnection(Config.Db.Risk)); }
            set { _risk = value; }
        }

        public dynamic Bi
        {
            get { return _bi ?? (_bi = Database.OpenConnection(Config.Db.Bi)); }
            set { _bi = value; }
        }

        public dynamic BankGateway
        {
            get { return _bankGateway ?? (_bankGateway = Database.OpenConnection(Config.Db.BankGateway)); }
            set { _bankGateway = value; }
        }

        public dynamic Blacklist
        {
            get { return _blacklist ?? (_blacklist = Database.OpenConnection(Config.Db.Blacklist)); }
            set { _blacklist = value; }
        }

        public dynamic CallReport
        {
            get { return _callReport ?? (_callReport = Database.OpenConnection(Config.Db.CallReport)); }
            set { _callReport = value; }
        }

        public dynamic CallValidate
        {
            get { return _callValidate ?? (_callValidate = Database.OpenConnection(Config.Db.CallValidate)); }
            set { _callValidate = value; }
        }

        public dynamic CardPayment
        {
            get { return _cardPayment ?? (_cardPayment = Database.OpenConnection(Config.Db.CardPayment)); }
            set { _cardPayment = value; }
        }

        public dynamic ColdStorage
        {
            get { return _coldStorage ?? (_coldStorage = Database.OpenConnection(Config.Db.ColdStorage)); }
            set { _coldStorage = value; }
        }

        public dynamic ContactManagement
        {
            get { return _contactManagement ?? (_contactManagement = Database.OpenConnection(Config.Db.ContactManagement)); }
            set { _contactManagement = value; }
        }

        public dynamic Experian
        {
            get { return _experian ?? (_experian = Database.OpenConnection(Config.Db.Experian)); }
            set { _experian = value; }
        }

        public dynamic ExperianBulk
        {
            get { return _experianBulk ?? (_experianBulk = Database.OpenConnection(Config.Db.ExperianBulk)); }
            set { _experianBulk = value; }
        }

        public dynamic FileStorage
        {
            get { return _fileStorage ?? (_fileStorage = Database.OpenConnection(Config.Db.FileStorage)); }
            set { _fileStorage = value; }
        }

        public dynamic Hpi
        {
            get { return _hpi ?? (_hpi = Database.OpenConnection(Config.Db.Hpi)); }
            set { _hpi = value; }
        }

        public dynamic IpLookup
        {
            get { return _ipLookup ?? (_ipLookup = Database.OpenConnection(Config.Db.IpLookup)); }
            set { _ipLookup = value; }
        }

        public dynamic QaData
        {
            get { return _qaData ?? (_qaData = Database.OpenConnection(Config.Db.QaData)); }
            set { _qaData = value; }
        }

        public dynamic Salesforce
        {
            get { return _salesforce ?? (_salesforce = Database.OpenConnection(Config.Db.Salesforce)); }
            set { _salesforce = value; }
        }

        public dynamic Scheduler
        {
            get { return _scheduler ?? (_scheduler = Database.OpenConnection(Config.Db.Scheduler)); }
            set { _scheduler = value; }
        }

        public dynamic Sms
        {
            get { return _sms ?? (_sms = Database.OpenConnection(Config.Db.Sms)); }
            set { _sms = value; }
        }

        public dynamic TimeZone
        {
            get { return _timeZone ?? (_timeZone = Database.OpenConnection(Config.Db.TimeZone)); }
            set { _timeZone = value; }
        }

        public dynamic TransUnion
        {
            get { return _transUnion ?? (_transUnion = Database.OpenConnection(Config.Db.TransUnion)); }
            set { _transUnion = value; }
        }

        public dynamic Uru
        {
            get { return _uru ?? (_uru = Database.OpenConnection(Config.Db.Uru)); }
            set { _uru = value; }
        }

        public dynamic WongaPay
        {
            get { return _wongaPay ?? (_wongaPay = Database.OpenConnection(Config.Db.WongaPay)); }
            set { _wongaPay = value; }
        }
    }
}
