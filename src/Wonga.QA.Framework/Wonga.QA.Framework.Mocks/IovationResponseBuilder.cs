using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.Framework.Mocks
{
	public class IovationResponseBuilder
	{
		private static readonly dynamic IovationDataOutput = new DataDriver().QaData.Db.IovationDataOutput;

		public IovationMockResponse BlackBox { get; private set; }

		public string DeviceAlias { get; private set; }

		public static IovationResponseBuilder New()
		{
			return new IovationResponseBuilder();
		}

		public IovationResponseBuilder ForBlackBox(IovationMockResponse blackBox)
		{
			BlackBox = blackBox;
			return this;
		}

		public IovationResponseBuilder UseDeviceAlias(string deviceAlias)
		{
			DeviceAlias = deviceAlias;
			return this;
		}

		public void OnResponseBasedOn(IovationMockResponse templateBlackBox)
		{			
			XElement document = GetExpectedResponse(templateBlackBox.ToString());

			const string nameSpace = @"http://schemas.microsoft.com/2003/10/Serialization/Arrays";

			string fullNodeName = string.Format("{{{0}}}Value", nameSpace);
			XElement deviceAliasValue = document.Descendants(fullNodeName).First();

			deviceAliasValue.Value = DeviceAlias;

			UpsertExpectedResponse(BlackBox.ToString(), document);
		}

		private XElement GetExpectedResponse(string type)
		{
			string xmlResponse = IovationDataOutput.FindByType(type).Response.ToString();

			return XElement.Parse(xmlResponse);
		}

		private void UpsertExpectedResponse(string type, XElement response)
		{
			var xmlResponse = response.ToString();

			IovationDataOutput.UpsertByType(Type: type, WaitTimeInSeconds: 0, Response: xmlResponse);
		}
	}
}
