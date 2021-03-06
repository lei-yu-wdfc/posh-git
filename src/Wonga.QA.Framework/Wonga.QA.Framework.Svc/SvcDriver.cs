﻿using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Svc
{
    public class SvcDriver
    {
        private SvcService _ops;
        private SvcService _comms;
        private SvcService _payments;
        private SvcService _marketing;
        private SvcService _risk;
        private SvcService _bi;

        private SvcService _bankGateway;
        private SvcService _blacklist;
        private SvcService _bankGatewayBmo;
        private SvcService _bankGatewayBottomLine;
        private SvcService _callReport;
        private SvcService _callValidate;
        private SvcService _cardPayment;
        private SvcService _coldStorage;
        private SvcService _contactManagement;
        private SvcService _documentGeneration;
        private SvcService _email;
        private SvcService _equifax;
        private SvcService _experian;
        private SvcService _experianBulk;
        private SvcService _fileStorage;
        private SvcService _graydon;
        private SvcService _hpi;
        private SvcService _bankGatewayHsbc;
        private SvcService __bankGatewayHyphen;
        private SvcService _iovation;
        private SvcService __bankGatewayRbc;
        private SvcService _salesforce;
        private SvcService _scheduler;
        private SvcService __bankGatewayScotia;
        private SvcService _sms;
        private SvcService _timeoutManager;
        private SvcService _timeZone;
        private SvcService _transUnion;
        private SvcService _uru;
        private SvcService _wongaPay;
        private SvcService __bankGatewayEasyPay;
    	private SvcService _payU;

        public SvcService Ops
        {
            get { return _ops ?? (_ops = new SvcService(Config.Svc.Ops.Key, Config.Svc.Ops.Value)); }
            set { _ops = value; }
        }

        public SvcService Comms
        {
            get { return _comms ?? (_comms = new SvcService(Config.Svc.Comms.Key, Config.Svc.Comms.Value)); }
            set { _comms = value; }
        }

        public SvcService Payments
        {
            get { return _payments ?? (_payments = new SvcService(Config.Svc.Payments.Key, Config.Svc.Payments.Value)); }
            set { _payments = value; }
        }

        public SvcService Marketing
        {
            get { return _marketing ?? (_marketing = new SvcService(Config.Svc.Marketing.Key, Config.Svc.Marketing.Value)); }
            set { _marketing = value; }
        }

        public SvcService Risk
        {
            get { return _risk ?? (_risk = new SvcService(Config.Svc.Risk.Key, Config.Svc.Risk.Value)); }
            set { _risk = value; }
        }

        public SvcService Bi
        {
            get { return _bi ?? (_bi = new SvcService(Config.Svc.Bi.Key, Config.Svc.Bi.Value)); }
            set { _bi = value; }
        }

        public SvcService BankGateway
        {
            get { return _bankGateway ?? (_bankGateway = new SvcService(Config.Svc.BankGateway.Key, Config.Svc.BankGateway.Value)); }
            set { _bankGateway = value; }
        }

        public SvcService Blacklist
        {
            get { return _blacklist ?? (_blacklist = new SvcService(Config.Svc.Blacklist.Key, Config.Svc.Blacklist.Value)); }
            set { _blacklist = value; }
        }

        public SvcService BankGatewayBmo
        {
            get { return _bankGatewayBmo ?? (_bankGatewayBmo = new SvcService(Config.Svc.BankGatewayBmo.Key, Config.Svc.BankGatewayBmo.Value)); }
            set { _bankGatewayBmo = value; }
        }

        public SvcService BankGatewayBottomLine
        {
            get { return _bankGatewayBottomLine ?? (_bankGatewayBottomLine = new SvcService(Config.Svc.BankGatewayBottomLine.Key, Config.Svc.BankGatewayBottomLine.Value)); }
            set { _bankGatewayBottomLine = value; }
        }

        public SvcService CallReport
        {
            get { return _callReport ?? (_callReport = new SvcService(Config.Svc.CallReport.Key, Config.Svc.CallReport.Value)); }
            set { _callReport = value; }
        }

        public SvcService CallValidate
        {
            get { return _callValidate ?? (_callValidate = new SvcService(Config.Svc.CallValidate.Key, Config.Svc.CallValidate.Value)); }
            set { _callValidate = value; }
        }

        public SvcService CardPayment
        {
            get { return _cardPayment ?? (_cardPayment = new SvcService(Config.Svc.CardPayment.Key, Config.Svc.CardPayment.Value)); }
            set { _cardPayment = value; }
        }

        public SvcService ColdStorage
        {
            get { return _coldStorage ?? (_coldStorage = new SvcService(Config.Svc.ColdStorage.Key, Config.Svc.ColdStorage.Value)); }
            set { _coldStorage = value; }
        }

        public SvcService ContactManagement
        {
            get { return _contactManagement ?? (_contactManagement = new SvcService(Config.Svc.ContactManagement.Key, Config.Svc.ContactManagement.Value)); }
            set { _contactManagement = value; }
        }

        public SvcService DocumentGeneration
        {
            get { return _documentGeneration ?? (_documentGeneration = new SvcService(Config.Svc.DocumentGeneration.Key, Config.Svc.DocumentGeneration.Value)); }
            set { _documentGeneration = value; }
        }

        public SvcService Email
        {
            get { return _email ?? (_email = new SvcService(Config.Svc.Email.Key, Config.Svc.Email.Value)); }
            set { _email = value; }
        }

        public SvcService Equifax
        {
            get { return _equifax ?? (_equifax = new SvcService(Config.Svc.Equifax.Key, Config.Svc.Equifax.Value)); }
            set { _equifax = value; }
        }

        public SvcService Experian
        {
            get { return _experian ?? (_experian = new SvcService(Config.Svc.Experian.Key, Config.Svc.Experian.Value)); }
            set { _experian = value; }
        }

        public SvcService ExperianBulk
        {
            get { return _experianBulk ?? (_experianBulk = new SvcService(Config.Svc.ExperianBulk.Key, Config.Svc.ExperianBulk.Value)); }
            set { _experianBulk = value; }
        }

        public SvcService FileStorage
        {
            get { return _fileStorage ?? (_fileStorage = new SvcService(Config.Svc.FileStorage.Key, Config.Svc.FileStorage.Value)); }
            set { _fileStorage = value; }
        }

        public SvcService Graydon
        {
            get { return _graydon ?? (_graydon = new SvcService(Config.Svc.Graydon.Key, Config.Svc.Graydon.Value)); }
            set { _graydon = value; }
        }

        public SvcService BankGatewayHpi
        {
            get { return _hpi ?? (_hpi = new SvcService(Config.Svc.Hpi.Key, Config.Svc.Hpi.Value)); }
            set { _hpi = value; }
        }

        public SvcService BankGatewayHsbc
        {
            get { return _bankGatewayHsbc ?? (_bankGatewayHsbc = new SvcService(Config.Svc.BankGatewayHsbc.Key, Config.Svc.BankGatewayHsbc.Value)); }
            set { _bankGatewayHsbc = value; }
        }

        public SvcService BankGatewayHyphen
        {
            get { return __bankGatewayHyphen ?? (__bankGatewayHyphen = new SvcService(Config.Svc.BankGatewayHyphen.Key, Config.Svc.BankGatewayHyphen.Value)); }
            set { __bankGatewayHyphen = value; }
        }

        public SvcService Iovation
        {
            get { return _iovation ?? (_iovation = new SvcService(Config.Svc.Iovation.Key, Config.Svc.Iovation.Value)); }
            set { _iovation = value; }
        }

        public SvcService BankGatewayRbc
        {
            get { return __bankGatewayRbc ?? (__bankGatewayRbc = new SvcService(Config.Svc.BankGatewayRbc.Key, Config.Svc.BankGatewayRbc.Value)); }
            set { __bankGatewayRbc = value; }
        }

        public SvcService Salesforce
        {
            get { return _salesforce ?? (_salesforce = new SvcService(Config.Svc.Salesforce.Key, Config.Svc.Salesforce.Value)); }
            set { _salesforce = value; }
        }

        public SvcService Scheduler
        {
            get { return _scheduler ?? (_scheduler = new SvcService(Config.Svc.Scheduler.Key, Config.Svc.Scheduler.Value)); }
            set { _scheduler = value; }
        }

        public SvcService BankGatewayScotia
        {
            get { return __bankGatewayScotia ?? (__bankGatewayScotia = new SvcService(Config.Svc.BankGatewayScotia.Key, Config.Svc.BankGatewayScotia.Value)); }
            set { __bankGatewayScotia = value; }
        }

        public SvcService Sms
        {
            get { return _sms ?? (_sms = new SvcService(Config.Svc.Sms.Key, Config.Svc.Sms.Value)); }
            set { _sms = value; }
        }

        public SvcService TimeoutManager
        {
            get { return _timeoutManager ?? (_timeoutManager = new SvcService(Config.Svc.TimeoutManager.Key, Config.Svc.TimeoutManager.Value)); }
            set { _timeoutManager = value; }
        }

        public SvcService TimeZone
        {
            get { return _timeZone ?? (_timeZone = new SvcService(Config.Svc.TimeZone.Key, Config.Svc.TimeZone.Value)); }
            set { _timeZone = value; }
        }

        public SvcService TransUnion
        {
            get { return _transUnion ?? (_transUnion = new SvcService(Config.Svc.TransUnion.Key, Config.Svc.TransUnion.Value)); }
            set { _transUnion = value; }
        }

        public SvcService Uru
        {
            get { return _uru ?? (_uru = new SvcService(Config.Svc.Uru.Key, Config.Svc.Uru.Value)); }
            set { _uru = value; }
        }

        public SvcService WongaPay
        {
            get { return _wongaPay ?? (_wongaPay = new SvcService(Config.Svc.WongaPay.Key, Config.Svc.WongaPay.Value)); }
            set { _wongaPay = value; }
        }

        public SvcService BankGatewayEasyPay
        {
            get { return __bankGatewayEasyPay ?? (__bankGatewayEasyPay = new SvcService(Config.Svc.BankGatewayEasyPay.Key, Config.Svc.BankGatewayEasyPay.Value)); }
            set { __bankGatewayEasyPay = value; }
        }

		public SvcService PayU
        {
			get { return _payU ?? (_payU = new SvcService(Config.Svc.PayU.Key, Config.Svc.PayU.Value)); }
			set { _payU = value; }
        }
    }
}
