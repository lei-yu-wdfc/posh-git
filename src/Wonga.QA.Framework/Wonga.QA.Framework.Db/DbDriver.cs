using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Db.Bi;
using Wonga.QA.Framework.Db.Blacklist;
using Wonga.QA.Framework.Db.CallReport;
using Wonga.QA.Framework.Db.CallValidate;
using Wonga.QA.Framework.Db.CardPayment;
using Wonga.QA.Framework.Db.ColdStorage;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.ContactManagement;
using Wonga.QA.Framework.Db.Experian;
using Wonga.QA.Framework.Db.ExperianBulk;
using Wonga.QA.Framework.Db.FileStorage;
using Wonga.QA.Framework.Db.Hpi;
using Wonga.QA.Framework.Db.IpLookup;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsLogs;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Db.Salesforce;
using Wonga.QA.Framework.Db.Scheduler;
using Wonga.QA.Framework.Db.Sms;
using Wonga.QA.Framework.Db.TimeZone;
using Wonga.QA.Framework.Db.TransUnion;
using Wonga.QA.Framework.Db.Uru;
using Wonga.QA.Framework.Db.WongaPay;

namespace Wonga.QA.Framework.Db
{
    public class DbDriver
    {
        private OpsDatabase _ops;
        private OpsSagasDatabase _opsSagas;
        private OpsLogsDatabase _opsLogs;
        private CommsDatabase _comms;
        private PaymentsDatabase _payments;
        private RiskDatabase _risk;
        private BiDatabase _bi;
        private BankGatewayDatabase _bankGateway;
        private BlacklistDatabase _blacklist;
        private CallReportDatabase _callReport;
        private CallValidateDatabase _callValidate;
        private CardPaymentDatabase _cardPayment;
        private ColdStorageDatabase _coldStorage;
        private ContactManagementDatabase _contactManagement;
        private ExperianDatabase _experian;
        private ExperianBulkDatabase _experianBulk;
        private FileStorageDatabase _fileStorage;
        private HpiDatabase _hpi;
        private IpLookupDatabase _ipLookup;
        private SalesforceDatabase _salesforce;
        private SchedulerDatabase _scheduler;
        private SmsDatabase _sms;
        private TimeZoneDatabase _timeZone;
        private TransUnionDatabase _transUnion;
        private UruDatabase _uru;
        private WongaPayDatabase _wongaPay;

        public OpsDatabase Ops
        {
            get { return _ops ?? (_ops = new OpsDatabase(Config.Db.Ops)); }
            set { _ops = value; }
        }

        public OpsSagasDatabase OpsSagas
        {
            get { return _opsSagas ?? (_opsSagas = new OpsSagasDatabase(Config.Db.OpsSagas)); }
            set { _opsSagas = value; }
        }

        public OpsLogsDatabase OpsLogs
        {
            get { return _opsLogs ?? (_opsLogs = new OpsLogsDatabase(Config.Db.OpsLogs)); }
            set { _opsLogs = value; }
        }

        public CommsDatabase Comms
        {
            get { return _comms ?? (_comms = new CommsDatabase(Config.Db.Comms)); }
            set { _comms = value; }
        }

        public PaymentsDatabase Payments
        {
            get { return _payments ?? (_payments = new PaymentsDatabase(Config.Db.Payments)); }
            set { _payments = value; }
        }

        public RiskDatabase Risk
        {
            get { return _risk ?? (_risk = new RiskDatabase(Config.Db.Risk)); }
            set { _risk = value; }
        }

        public BiDatabase Bi
        {
            get { return _bi ?? (_bi = new BiDatabase(Config.Db.Bi)); }
            set { _bi = value; }
        }

        public BankGatewayDatabase BankGateway
        {
            get { return _bankGateway ?? (_bankGateway = new BankGatewayDatabase(Config.Db.BankGateway)); }
            set { _bankGateway = value; }
        }

        public BlacklistDatabase Blacklist
        {
            get { return _blacklist ?? (_blacklist = new BlacklistDatabase(Config.Db.Blacklist)); }
            set { _blacklist = value; }
        }

        public CallReportDatabase CallReport
        {
            get { return _callReport ?? (_callReport = new CallReportDatabase(Config.Db.CallReport)); }
            set { _callReport = value; }
        }

        public CallValidateDatabase CallValidate
        {
            get { return _callValidate ?? (_callValidate = new CallValidateDatabase(Config.Db.CallValidate)); }
            set { _callValidate = value; }
        }

        public CardPaymentDatabase CardPayment
        {
            get { return _cardPayment ?? (_cardPayment = new CardPaymentDatabase(Config.Db.CardPayment)); }
            set { _cardPayment = value; }
        }

        public ColdStorageDatabase ColdStorage
        {
            get { return _coldStorage ?? (_coldStorage = new ColdStorageDatabase(Config.Db.ColdStorage)); }
            set { _coldStorage = value; }
        }

        public ContactManagementDatabase ContactManagement
        {
            get { return _contactManagement ?? (_contactManagement = new ContactManagementDatabase(Config.Db.ContactManagement)); }
            set { _contactManagement = value; }
        }

        public ExperianDatabase Experian
        {
            get { return _experian ?? (_experian = new ExperianDatabase(Config.Db.Experian)); }
            set { _experian = value; }
        }

        public ExperianBulkDatabase ExperianBulk
        {
            get { return _experianBulk ?? (_experianBulk = new ExperianBulkDatabase(Config.Db.ExperianBulk)); }
            set { _experianBulk = value; }
        }

        public FileStorageDatabase FileStorage
        {
            get { return _fileStorage ?? (_fileStorage = new FileStorageDatabase(Config.Db.FileStorage)); }
            set { _fileStorage = value; }
        }

        public HpiDatabase Hpi
        {
            get { return _hpi ?? (_hpi = new HpiDatabase(Config.Db.Hpi)); }
            set { _hpi = value; }
        }

        public IpLookupDatabase IpLookup
        {
            get { return _ipLookup ?? (_ipLookup = new IpLookupDatabase(Config.Db.IpLookup)); }
            set { _ipLookup = value; }
        }

        public SalesforceDatabase Salesforce
        {
            get { return _salesforce ?? (_salesforce = new SalesforceDatabase(Config.Db.Salesforce)); }
            set { _salesforce = value; }
        }

        public SchedulerDatabase Scheduler
        {
            get { return _scheduler ?? (_scheduler = new SchedulerDatabase(Config.Db.Scheduler)); }
            set { _scheduler = value; }
        }

        public SmsDatabase Sms
        {
            get { return _sms ?? (_sms = new SmsDatabase(Config.Db.Sms)); }
            set { _sms = value; }
        }

        public TimeZoneDatabase TimeZone
        {
            get { return _timeZone ?? (_timeZone = new TimeZoneDatabase(Config.Db.TimeZone)); }
            set { _timeZone = value; }
        }

        public TransUnionDatabase TransUnion
        {
            get { return _transUnion ?? (_transUnion = new TransUnionDatabase(Config.Db.TransUnion)); }
            set { _transUnion = value; }
        }

        public UruDatabase Uru
        {
            get { return _uru ?? (_uru = new UruDatabase(Config.Db.Uru)); }
            set { _uru = value; }
        }

        public WongaPayDatabase WongaPay
        {
            get { return _wongaPay ?? (_wongaPay = new WongaPayDatabase(Config.Db.WongaPay)); }
            set { _wongaPay = value; }
        }
    }
}
