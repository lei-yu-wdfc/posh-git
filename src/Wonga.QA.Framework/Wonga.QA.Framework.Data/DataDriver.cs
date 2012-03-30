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
        private Lazy<OpsDatabase> _ops = new Lazy<OpsDatabase>(() => new OpsDatabase(Config.Db.Ops));
        private Lazy<OpsSagasDatabase> _opsSagas = new Lazy<OpsSagasDatabase>(() => new OpsSagasDatabase(Config.Db.OpsSagas));
        private Lazy<OpsLogsDatabase> _opsLogs = new Lazy<OpsLogsDatabase>(() => new OpsLogsDatabase(Config.Db.OpsLogs));
        private Lazy<CommsDatabase> _comms = new Lazy<CommsDatabase>(() => new CommsDatabase(Config.Db.Comms));
        private Lazy<PaymentsDatabase> _payments = new Lazy<PaymentsDatabase>(() => new PaymentsDatabase(Config.Db.Payments));
        private Lazy<RiskDatabase> _risk = new Lazy<RiskDatabase>(() => new RiskDatabase(Config.Db.Risk));
        private Lazy<BiDatabase> _bi = new Lazy<BiDatabase>(() => new BiDatabase(Config.Db.Bi));
        private Lazy<BankGatewayDatabase> _bankGateway = new Lazy<BankGatewayDatabase>(() => new BankGatewayDatabase(Config.Db.BankGateway));
        private Lazy<BlacklistDatabase> _blacklist = new Lazy<BlacklistDatabase>(() => new BlacklistDatabase(Config.Db.Blacklist));
        private Lazy<CallReportDatabase> _callReport = new Lazy<CallReportDatabase>(() => new CallReportDatabase(Config.Db.CallReport));
        private Lazy<CallValidateDatabase> _callValidate = new Lazy<CallValidateDatabase>(() => new CallValidateDatabase(Config.Db.CallValidate));
        private Lazy<CardPaymentDatabase> _cardPayment = new Lazy<CardPaymentDatabase>(() => new CardPaymentDatabase(Config.Db.CardPayment));
        private Lazy<ColdStorageDatabase> _coldStorage = new Lazy<ColdStorageDatabase>(() => new ColdStorageDatabase(Config.Db.ColdStorage));
        private Lazy<ContactManagementDatabase> _contactManagement = new Lazy<ContactManagementDatabase>(() => new ContactManagementDatabase(Config.Db.ContactManagement));
        private Lazy<ExperianDatabase> _experian = new Lazy<ExperianDatabase>(() => new ExperianDatabase(Config.Db.Experian));
        private Lazy<ExperianBulkDatabase> _experianBulk = new Lazy<ExperianBulkDatabase>(() => new ExperianBulkDatabase(Config.Db.ExperianBulk));
        private Lazy<FileStorageDatabase> _fileStorage = new Lazy<FileStorageDatabase>(() => new FileStorageDatabase(Config.Db.FileStorage));
        private Lazy<HpiDatabase> _hpi = new Lazy<HpiDatabase>(() => new HpiDatabase(Config.Db.Hpi));
        private Lazy<IpLookupDatabase> _ipLookup = new Lazy<IpLookupDatabase>(() => new IpLookupDatabase(Config.Db.IpLookup));
        private Lazy<QaDataDatabase> _qaData = new Lazy<QaDataDatabase>(() => new QaDataDatabase(Config.Db.QaData));
        private Lazy<SalesforceDatabase> _salesforce = new Lazy<SalesforceDatabase>(() => new SalesforceDatabase(Config.Db.Salesforce));
        private Lazy<SchedulerDatabase> _scheduler = new Lazy<SchedulerDatabase>(() => new SchedulerDatabase(Config.Db.Scheduler));
        private Lazy<SmsDatabase> _sms = new Lazy<SmsDatabase>(() => new SmsDatabase(Config.Db.Sms));
        private Lazy<TimeZoneDatabase> _timeZone = new Lazy<TimeZoneDatabase>(() => new TimeZoneDatabase(Config.Db.TimeZone));
        private Lazy<TransUnionDatabase> _transUnion = new Lazy<TransUnionDatabase>(() => new TransUnionDatabase(Config.Db.TransUnion));
        private Lazy<UruDatabase> _uru = new Lazy<UruDatabase>(() => new UruDatabase(Config.Db.Uru));
        private Lazy<WongaPayDatabase> _wongaPay = new Lazy<WongaPayDatabase>(() => new WongaPayDatabase(Config.Db.WongaPay));

        public OpsSagasDatabase OpsSagas { get { return _opsSagas.Value;} }
        public OpsDatabase Ops { get { return _ops.Value; } }
        public OpsLogsDatabase OpsLogs { get { return _opsLogs.Value; } }
        public CommsDatabase Comms { get { return _comms.Value; } }
        public PaymentsDatabase Payments { get { return _payments.Value; } }
        public RiskDatabase Risk { get { return _risk.Value; } }
        public BiDatabase Bi { get { return _bi.Value; } }
        public BankGatewayDatabase BankGateway { get { return _bankGateway.Value; } }
        public BlacklistDatabase Blacklist { get { return _blacklist.Value; } }
        public CallReportDatabase CallReport { get { return _callReport.Value; } }
        public CallValidateDatabase CallValidate { get { return _callValidate.Value; } }
        public CardPaymentDatabase CardPayment { get { return _cardPayment.Value; } }
        public ColdStorageDatabase ColdStorage { get { return _coldStorage.Value; } }
        public ContactManagementDatabase ContactManagement { get { return _contactManagement.Value; } }
        public ExperianDatabase Experian { get { return _experian.Value; } }
        public ExperianBulkDatabase ExperianBulk { get { return _experianBulk.Value; } }
        public FileStorageDatabase FileStorage { get { return _fileStorage.Value; } }
        public HpiDatabase Hpi { get { return _hpi.Value; } }
        public IpLookupDatabase IpLookup { get { return _ipLookup.Value; } }
        public QaDataDatabase QaData { get { return _qaData.Value; } }
        public SalesforceDatabase Salesforce { get { return _salesforce.Value; }}
        public SchedulerDatabase Scheduler { get { return _scheduler.Value; } }
        public SmsDatabase Sms { get { return _sms.Value; }}
        public TimeZoneDatabase TimeZone { get { return _timeZone.Value; } }
        public TransUnionDatabase TransUnion { get { return _transUnion.Value; } }
        public UruDatabase Uru { get { return _uru.Value; } }
        public WongaPayDatabase WongaPay { get { return _wongaPay.Value; } }
    }
}
