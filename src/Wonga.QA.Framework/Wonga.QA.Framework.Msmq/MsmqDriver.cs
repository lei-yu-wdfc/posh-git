﻿using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    public class MsmqDriver
    {
        private MsmqQueue _ops;
        private MsmqQueue _comms;
        private MsmqQueue _payments;
        private MsmqQueue _risk;
        private MsmqQueue _marketing;
        private MsmqQueue _bi;
        private MsmqQueue _bankGateway;
		private MsmqQueue _bankGatewayBmo;
		private MsmqQueue _bankGatewayRbc;
		private MsmqQueue _bankGatewayScotia;
		private MsmqQueue _blacklist;
        private MsmqQueue _callReport;
        private MsmqQueue _callValidate;
        private MsmqQueue _cardPayment;
        private MsmqQueue _coldStorage;
        private MsmqQueue _email;
        private MsmqQueue _equifax;
        private MsmqQueue _experian;
        private MsmqQueue _experianBulk;
        private MsmqQueue _fileStorage;
        private MsmqQueue _graydon;
        private MsmqQueue _hpi;
        private MsmqQueue _BankGatewayHsbc;
        private MsmqQueue _BankGatewayHyphen;
        private MsmqQueue _iovation;
        private MsmqQueue _salesforce;
        private MsmqQueue _sms;
        private MsmqQueue _smsDistributor;
        private MsmqQueue _timeZone;
        private MsmqQueue _transUnion;
        private MsmqQueue _uru;
        private MsmqQueue _wongaPay;
        private MsmqQueue _BankGatewayEasyPay;

        public MsmqQueue BankGatewayEasyPay
        {
            get { return _BankGatewayEasyPay ?? (_BankGatewayEasyPay = new MsmqQueue(Config.Msmq.BankGatewayEasyPay)); }
            set { _BankGatewayEasyPay = value; }
        }

        public MsmqQueue Ops
        {
            get { return _ops ?? (_ops = new MsmqQueue(Config.Msmq.Ops)); }
            set { _ops = value; }
        }

        public MsmqQueue Comms
        {
            get { return _comms ?? (_comms = new MsmqQueue(Config.Msmq.Comms)); }
            set { _comms = value; }
        }

        public MsmqQueue Payments
        {
            get { return _payments ?? (_payments = new MsmqQueue(Config.Msmq.Payments)); }
            set { _payments = value; }
        }

        public MsmqQueue Risk
        {
            get { return _risk ?? (_risk = new MsmqQueue(Config.Msmq.Risk)); }
            set { _risk = value; }
        }

        public MsmqQueue Marketing
        {
            get { return _marketing ?? (_marketing = new MsmqQueue(Config.Msmq.Marketing)); }
            set { _marketing = value; }
        }

        public MsmqQueue Bi
        {
            get { return _bi ?? (_bi = new MsmqQueue(Config.Msmq.Bi)); }
            set { _bi = value; }
        }

        public MsmqQueue BankGateway
        {
            get { return _bankGateway ?? (_bankGateway = new MsmqQueue(Config.Msmq.BankGateway)); }
            set { _bankGateway = value; }
        }

		public MsmqQueue BankGatewayBmo
		{
			get { return _bankGatewayBmo ?? (_bankGatewayBmo = new MsmqQueue(Config.Msmq.BankGatewayBmo)); }
			set { _bankGatewayBmo = value; }
		}

		public MsmqQueue BankGatewayRbc
		{
			get { return _bankGatewayRbc ?? (_bankGatewayRbc = new MsmqQueue(Config.Msmq.BankGatewayRbc)); }
			set { _bankGatewayRbc = value; }
		}

        public MsmqQueue Blacklist
        {
            get { return _blacklist ?? (_blacklist = new MsmqQueue(Config.Msmq.Blacklist)); }
            set { _blacklist = value; }
        }

        public MsmqQueue CallReport
        {
            get { return _callReport ?? (_callReport = new MsmqQueue(Config.Msmq.CallReport)); }
            set { _callReport = value; }
        }

        public MsmqQueue CallValidate
        {
            get { return _callValidate ?? (_callValidate = new MsmqQueue(Config.Msmq.CallValidate)); }
            set { _callValidate = value; }
        }

        public MsmqQueue CardPayment
        {
            get { return _cardPayment ?? (_cardPayment = new MsmqQueue(Config.Msmq.CardPayment)); }
            set { _cardPayment = value; }
        }

        public MsmqQueue ColdStorage
        {
            get { return _coldStorage ?? (_coldStorage = new MsmqQueue(Config.Msmq.ColdStorage)); }
            set { _coldStorage = value; }
        }

        public MsmqQueue Email
        {
            get { return _email ?? (_email = new MsmqQueue(Config.Msmq.Email)); }
            set { _email = value; }
        }

        public MsmqQueue Equifax
        {
            get { return _equifax ?? (_equifax = new MsmqQueue(Config.Msmq.Equifax)); }
            set { _equifax = value; }
        }

        public MsmqQueue Experian
        {
            get { return _experian ?? (_experian = new MsmqQueue(Config.Msmq.Experian)); }
            set { _experian = value; }
        }

        public MsmqQueue ExperianBulk
        {
            get { return _experianBulk ?? (_experianBulk = new MsmqQueue(Config.Msmq.ExperianBulk)); }
            set { _experianBulk = value; }
        }

        public MsmqQueue FileStorage
        {
            get { return _fileStorage ?? (_fileStorage = new MsmqQueue(Config.Msmq.FileStorage)); }
            set { _fileStorage = value; }
        }

        public MsmqQueue Graydon
        {
            get { return _graydon ?? (_graydon = new MsmqQueue(Config.Msmq.Graydon)); }
            set { _graydon = value; }
        }

        public MsmqQueue Hpi
        {
            get { return _hpi ?? (_hpi = new MsmqQueue(Config.Msmq.Hpi)); }
            set { _hpi = value; }
        }

        public MsmqQueue BankGatewayHsbc
        {
            get { return _BankGatewayHsbc ?? (_BankGatewayHsbc = new MsmqQueue(Config.Msmq.BankGatewayHsbc)); }
            set { _BankGatewayHsbc = value; }
        }

        public MsmqQueue BankGatewayHyphen
        {
            get { return _BankGatewayHyphen ?? (_BankGatewayHyphen = new MsmqQueue(Config.Msmq.BankGatewayHyphen)); }
            set { _BankGatewayHyphen = value; }
        }

        public MsmqQueue Iovation
        {
            get { return _iovation ?? (_iovation = new MsmqQueue(Config.Msmq.Iovation)); }
            set { _iovation = value; }
        }

        public MsmqQueue Salesforce
        {
            get { return _salesforce ?? (_salesforce = new MsmqQueue(Config.Msmq.Salesforce)); }
            set { _salesforce = value; }
        }

        public MsmqQueue BankGatewayScotia
        {
            get { return _bankGatewayScotia ?? (_bankGatewayScotia = new MsmqQueue(Config.Msmq.BankGatewayScotia)); }
            set { _bankGatewayScotia = value; }
        }

        public MsmqQueue Sms
        {
            get { return _sms ?? (_sms = new MsmqQueue(Config.Msmq.Sms)); }
            set { _sms = value; }
        }

        public MsmqQueue SmsDistrubutor
        {
            get { return _smsDistributor ?? (_smsDistributor = new MsmqQueue(Config.Msmq.SmsDistributor)); }
            set { _smsDistributor = value; }
        }

        public MsmqQueue TimeZone
        {
            get { return _timeZone ?? (_timeZone = new MsmqQueue(Config.Msmq.Timezone)); }
            set { _timeZone = value; }
        }

        public MsmqQueue TransUnion
        {
            get { return _transUnion ?? (_transUnion = new MsmqQueue(Config.Msmq.TransUnion)); }
            set { _transUnion = value; }
        }

        public MsmqQueue Uru
        {
            get { return _uru ?? (_uru = new MsmqQueue(Config.Msmq.Uru)); }
            set { _uru = value; }
        }

        public MsmqQueue WongaPay
        {
            get { return _wongaPay ?? (_wongaPay = new MsmqQueue(Config.Msmq.WongaPay)); }
            set { _wongaPay = value; }
        }
    }
}
