using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class BehaviourAndAdvertTracking : UiTest
	{
		#region DoubleClickTags

    	private static readonly List<KeyValuePair<string, string>> DcTagsHomePage =
    		new List<KeyValuePair<string, string>>()
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
    				new KeyValuePair<string, string>("cat", "za_ho244"),
    				new KeyValuePair<string, string>("type", "homepage"),
    				new KeyValuePair<string, string>("paths", "<front>"),
    				new KeyValuePair<string, string>("paths", "home"),
    				new KeyValuePair<string, string>("paths", "homepages/*"),
    				new KeyValuePair<string, string>("group_name", "Homepage"),
    				new KeyValuePair<string, string>("activity_name", "ZA_Homepage"),
    				new KeyValuePair<string, string>("activity_id", "995820"),
    				new KeyValuePair<string, string>("html_override", "")
    			};

    	private static readonly List<KeyValuePair<string, string>> DcTagsPersonalDetailsPage = new List<KeyValuePair<string, string>>
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
					new KeyValuePair<string, string>("cat", "za_l0571"),
					new KeyValuePair<string, string>("type", "za_l0"),
					new KeyValuePair<string, string>("paths", "apply-details"),
					new KeyValuePair<string, string>("group_name", "ZA_L0"),
					new KeyValuePair<string, string>("activity_name", "ZA_L0_PersonalDetails"),
					new KeyValuePair<string, string>("activity_id", "995827"),
					new KeyValuePair<string, string>("html_override", "")
    			};

		private static readonly List<KeyValuePair<string, string>> DcTagsAddressDetailsPage = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("src", "3567941"),
					new KeyValuePair<string, string>("cat", "za_l0642"),
					new KeyValuePair<string, string>("type", "za_l0"),
					new KeyValuePair<string, string>("paths", "apply-address"),
					new KeyValuePair<string, string>("group_name", "ZA_L0"),
					new KeyValuePair<string, string>("activity_name", "ZA_L0_Address"),
					new KeyValuePair<string, string>("activity_id", "995828"),
					new KeyValuePair<string, string>("html_override", "")
				};

    	private static readonly List<KeyValuePair<string, string>> DcTagsAccountDetailsPage =
    		new List<KeyValuePair<string, string>>()
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
    				new KeyValuePair<string, string>("cat", "za_l0281"),
    				new KeyValuePair<string, string>("type", "za_l0"),
    				new KeyValuePair<string, string>("paths", "apply-account"),
    				new KeyValuePair<string, string>("group_name", "ZA_L0"),
    				new KeyValuePair<string, string>("activity_name", "ZA_L0_Account"),
    				new KeyValuePair<string, string>("activity_id", "995832"),
    				new KeyValuePair<string, string>("html_override", "")
    			};

    	private static readonly List<KeyValuePair<string, string>> DcTagsPersonalBankAccountPage = new List<KeyValuePair<string, string>>
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
    				new KeyValuePair<string, string>("cat", "za_l0346"),
    				new KeyValuePair<string, string>("type", "za_l0"),
    				new KeyValuePair<string, string>("paths", "apply-bank"),
    				new KeyValuePair<string, string>("group_name", "ZA_L0"),
    				new KeyValuePair<string, string>("activity_name", "ZA_L0_Bank"),
    				new KeyValuePair<string, string>("activity_id", "996615"),
    				new KeyValuePair<string, string>("html_override", "")
    			};

    	private static readonly List<KeyValuePair<string, string>> DcTagsProcessingPage = new List<KeyValuePair<string, string>>
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
    				new KeyValuePair<string, string>("cat", "za_l0882"),
    				new KeyValuePair<string, string>("type", "za_l0"),
    				new KeyValuePair<string, string>("paths", "processing"),
    				new KeyValuePair<string, string>("group_name", "ZA_L0"),
    				new KeyValuePair<string, string>("activity_name", "ZA_L0_Processing"),
    				new KeyValuePair<string, string>("activity_id", "996616"),
    				new KeyValuePair<string, string>("html_override", "")
    			};


    	private static readonly List<KeyValuePair<string, string>> DcTagsAcceptedPage = new List<KeyValuePair<string, string>>
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
    				new KeyValuePair<string, string>("cat", "za_l0817"),
    				new KeyValuePair<string, string>("type", "za_l0"),
    				new KeyValuePair<string, string>("paths", "apply-accept"),
    				new KeyValuePair<string, string>("group_name", "ZA_L0"),
    				new KeyValuePair<string, string>("activity_name", "ZA_L0_Accept"),
    				new KeyValuePair<string, string>("activity_id", "996618"),
    				new KeyValuePair<string, string>("html_override", "")
    			};

    	private static readonly List<KeyValuePair<string, string>> DcTagsDealDonePage = new List<KeyValuePair<string, string>>
    			{
    				new KeyValuePair<string, string>("src", "3567941"),
					new KeyValuePair<string, string>("cat", "za_l0643"),
					new KeyValuePair<string, string>("type", "za_l0_s"),
					new KeyValuePair<string, string>("paths", "deal-done"),
					new KeyValuePair<string, string>("group_name", "ZA_L0_Sale"),
					new KeyValuePair<string, string>("activity_name", "ZA_L0_DealDone"),
					new KeyValuePair<string, string>("activity_id", "996620"),
					new KeyValuePair<string, string>("html_override", "")
    			};

		private static readonly List<KeyValuePair<string, string>> DcTagsMyAccountPage = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("src", "3567941"),
					new KeyValuePair<string, string>("cat", "za_ln060"),
					new KeyValuePair<string, string>("type", "za_ln"),
					new KeyValuePair<string, string>("paths", "my-account"),
					new KeyValuePair<string, string>("group_name", "ZA_Ln"),
					new KeyValuePair<string, string>("activity_name", "ZA_Ln_MyAccount"),
					new KeyValuePair<string, string>("activity_id", "996622"),
					new KeyValuePair<string, string>("html_override", "")
				};
	
			#endregion 
    }
}
