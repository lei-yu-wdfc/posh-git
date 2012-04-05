using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
	class EndToEndApplicationTests : UiTest
	{
		private Dictionary<string, string> _originalServiceConfiguration = new Dictionary<string, string>();
		private string _forename;
		private string _surname;
		private string _nationalId;
		private DateTime _dateOfBirth;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case (AUT.Za):
					{
						_originalServiceConfiguration.Add("Mocks.HyphenAHVWebServiceEnabled", Drive.Db.GetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled").Value);
						Drive.Db.SetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled", "false");

						_originalServiceConfiguration.Add("Mocks.IovationEnabled", "true");
						Drive.Db.SetServiceConfiguration("Mocks.IovationEnabled", "true");

						_forename = "ANITHA";
						_surname = "ESSACK";
						_nationalId = "5712190106083";
						_dateOfBirth = new DateTime(1957, 12, 19);
					}
					break;

				default:
					{
						return;
					}
			}
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Db.SetServiceConfigurations(_originalServiceConfiguration);
		}

		[Test, AUT(AUT.Za), Pending()]
		public void EndToEndApplicationZaL0()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home());

			journey.FirstName = _forename;
			journey.LastName = _surname;
			journey.NationalId = _nationalId;
			journey.DateOfBirth = _dateOfBirth;
			
			var processingPage = journey.ApplyForLoan(200, 10)
								 .FillPersonalDetails(employerNameMask: "Wonga")
								 .FillAddressDetails()
								 .FillAccountDetails()
								 .FillBankDetails()
								 .CurrentPage as ProcessingPage;

			var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
			acceptedPage.SignAgreementConfirm();
			acceptedPage.SignDirectDebitConfirm();
			var dealDone = acceptedPage.Submit();
		}
	}
}
