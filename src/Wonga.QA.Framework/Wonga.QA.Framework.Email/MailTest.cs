using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AE.Net.Mail;
using MbUnit.Framework;

namespace Wonga.QA.Framework.Email
{
    [TestFixture]
    public class MailTest
    {
        [Test]
        public void Test()
        {
            using (var imap = new ImapClient("imap.gmail.com", "qa.wonga.com@gmail.com", "Allw0nga", ImapClient.AuthMethods.Login, 993, true))
            {
                var msgs = imap.SearchMessages(
                  SearchCondition.Undeleted().And(
                    //SearchCondition.From("david"),
                    //SearchCondition.SentSince(new DateTime(2000, 1, 1))
                    SearchCondition.Body("hSVfIiIJkppPMde")
                  )
                );

                Assert.AreEqual(msgs[0].Value.Subject, "This is cool!");

                //imap.NewMessage += (sender, e) =>
                //{
                //    var msg = imap.GetMessage(e.MessageCount - 1);
                //    Assert.AreEqual(msg.Subject, "IDLE support?  Yes, please!");
                //};
            }
        }
    }
}
