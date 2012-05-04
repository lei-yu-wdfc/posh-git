using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AE.Net.Mail;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Email
{
    public class EmailDriver
    {
        private Lazy<EmailAddress> _qa = new Lazy<EmailAddress>(() => new EmailAddress(Config.Email.QA.Host, Config.Email.QA.Username, Config.Email.QA.Password, Config.Email.QA.IsSsl, Config.Email.QA.Port));
        public EmailAddress QA
        {
            get { return _qa.Value; }
        }

        public EmailDriver()
        {
            EmailMappings.Configure();
        }
    }
}
