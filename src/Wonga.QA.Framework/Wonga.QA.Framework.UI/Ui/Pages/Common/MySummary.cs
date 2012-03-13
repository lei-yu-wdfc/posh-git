using Wonga.QA.Framework.UI.Elements;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MySummary : BasePage
    {
        public MyAccountNavigationElement Navigation { get; set; }
        
        public MySummary(UiClient client) : base(client)
        {
            Navigation = new MyAccountNavigationElement(this);
        }
    }
}