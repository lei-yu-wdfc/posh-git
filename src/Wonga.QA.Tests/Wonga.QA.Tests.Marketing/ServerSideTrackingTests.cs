//using System;
//using MbUnit.Framework;
//using Wonga.QA.Framework;
//using Wonga.QA.Framework.Api.Requests;
//using Wonga.QA.Framework.Api.Requests.Marketing.Commands;
//using Wonga.QA.Framework.Core;
//using Wonga.QA.Tests.Core;

//namespace Wonga.QA.Tests.Marketing
//{
//    [Parallelizable(TestScope.All)]
//    public class ServerSideTrackingTests
//    {

//        private static readonly String DEVICE_TYPE = "IPhone 4S";
//        private static readonly String DEVICE_GROUP = "Mobile";
//        private static readonly String ANOTHER_DEVICE_TYPE = "Another type";
//        private static readonly String ANOTHER_DEVICE_GROUP = "Another device";

//        private static readonly String DEFAULT_URI = "https://www.wonga.com";
//        private static readonly Guid[] guids = new Guid[3];

//        private static readonly dynamic _marketingDb = Drive.Data.Marketing.Db;


//        [Test, AUT(AUT.Uk), JIRA("UK-2067")]
//        public void SaveServerSideInitialTrackingDataCommandTest()
//        {
//            String sessionId = Get.GetId(Guid.NewGuid());
//            Guid sessionIdGuid = Guid.Parse(sessionId);

//            guids[0] = Guid.NewGuid();
//            guids[1] = Guid.NewGuid();
//            guids[2] = Guid.NewGuid();

//            var validCommand = new SaveServerSideInitialTrackingDataCommand
//            {
//                SessionId = sessionId,
//                Uri = BuildUri(true, guids)
//            };
//            var invalidCommand = new SaveServerSideInitialTrackingDataCommand
//            {
//                SessionId = Guid.Empty,
//                Uri = BuildUri(false, guids)
//            };

//            Drive.Api.Commands.Post(validCommand);
//            Assert.Throws<Exception>(() => Drive.Api.Commands.Post(invalidCommand));

//            var serverSideInitialtracingRecord = Do.Until(() => _marketingDb.ServerSideInitialTracking.FindBySessionId(sessionIdGuid));

//            Assert.AreEqual(serverSideInitialtracingRecord.A, guids[0].ToString());
//            Assert.AreEqual(serverSideInitialtracingRecord.B, guids[1].ToString());
//            Assert.AreEqual(serverSideInitialtracingRecord.F, guids[2].ToString());
//        }

//        [Test, AUT(AUT.Uk), JIRA("UK-2075")]
//        public void SaveServerSidePageTrackingDataCommandTest()
//        {
//            String sessionId = Get.GetId(Guid.NewGuid());
//            Guid sessionIdGuid = Guid.Parse(sessionId);

//            Guid accountId = Guid.NewGuid();
//            Guid applicationId = Guid.NewGuid();

//            var validCommand = new SaveServerSidePageTrackingDataCommand
//            {
//                AccountId = accountId,
//                ApplicationId = applicationId,
//                SessionId = sessionId,
//                Uri = DEFAULT_URI
//            };

//            Drive.Api.Commands.Post(validCommand);
//            var serversideTrackingRecord = Do.Until(() => _marketingDb.ServerSidePageTracking.FindBySessionId((sessionIdGuid)));

//            Assert.IsTrue(DEFAULT_URI.Equals(serversideTrackingRecord.Uri));
//            Assert.IsTrue(accountId.Equals(serversideTrackingRecord.AccountId));
//            Assert.IsTrue(applicationId.Equals(serversideTrackingRecord.ApplicationId));
//        }

//        [Test, AUT(AUT.Uk), JIRA("UK-2067, UK-2075")]
//        public void SaveServerSideTrackingDeviceCommandTest()
//        {
//            String sessionId = Get.GetId(Guid.NewGuid());
//            Guid sessionIdGuid = Guid.Parse(sessionId);

//            var command = new SaveServerSideTrackingDeviceCommand
//            {
//                DeviceGroup = DEVICE_GROUP,
//                DeviceType = DEVICE_TYPE,
//                SessionId = sessionId
//            };

//            Drive.Api.Commands.Post(command);
//            var trackingDeviceRecord = Do.Until(() => _marketingDb.ServerSideTrackingDevice.FindBySessionId((sessionIdGuid)));

//            Assert.IsTrue(sessionIdGuid.Equals(trackingDeviceRecord.SessionId));
//            Assert.IsTrue(DEVICE_GROUP.Equals(trackingDeviceRecord.DeviceGroup));
//            Assert.IsTrue(DEVICE_TYPE.Equals(trackingDeviceRecord.DeviceType));
//        }

//        [Test, AUT(AUT.Uk), JIRA("UK-2067, UK-2075")]
//        public void SaveServerSideTrackingDeviceMultipleCommandTest()
//        {
//            String sessionId = Get.GetId(Guid.NewGuid());
//            Guid sessionIdGuid = Guid.Parse(sessionId);

//            var trackingDeviceCommand = new SaveServerSideTrackingDeviceCommand
//            {
//                DeviceGroup = DEVICE_GROUP,
//                DeviceType = DEVICE_TYPE,
//                SessionId = sessionId
//            };

//            var updateDeviceCommand = new SaveServerSideTrackingDeviceCommand
//            {
//                DeviceGroup = ANOTHER_DEVICE_GROUP,
//                DeviceType = ANOTHER_DEVICE_TYPE,
//                SessionId = sessionId
//            };

//            Drive.Api.Commands.Post(trackingDeviceCommand);
//            Drive.Api.Commands.Post(updateDeviceCommand);

//            var trackingDeviceRecord = Do.Until(() => _marketingDb.ServerSideTrackingDevice.FindBySessionId((sessionIdGuid)));
//            Assert.IsFalse(ANOTHER_DEVICE_TYPE.Equals(trackingDeviceRecord.DeviceType));
//            Assert.IsFalse(ANOTHER_DEVICE_GROUP.Equals(trackingDeviceRecord.DeviceGroup));
//        }

//        public String BuildUri(bool isAllParametrs, Guid[] guids)
//        {
//            String result = "";

//            if (isAllParametrs.Equals(true))
//            {
//                result = string.Format(DEFAULT_URI + "?A={0}&amp;B={1}&amp;F={2}", guids[0], guids[1], guids[2]);
//            }

//            else
//            {
//                result = string.Format(DEFAULT_URI + "?A={0}&amp;F={1}", guids[0], guids[1]);
//            }

//            return result;
//        }


//    }
//}
