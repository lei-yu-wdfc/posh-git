using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AE.Net.Mail;
using MbUnit.Framework;

namespace Wonga.QA.Framework.Email
{
    public class EmailAddress
    {
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isSsl;
        private readonly int _port;
        private readonly string _host;
        //private Lazy<ImapClient> _client;

        public EmailAddress(string host, string username, string password, bool isSsl, int port)
        {
            _username = username;
            _password = password;
            _isSsl = isSsl;
            _port = port;
            _host = host;
            //_client = new Lazy<ImapClient>(() => new ImapClient(_host, _username, _password, ImapClient.AuthMethods.Login, _port, _isSsl));
        }

        //For whatever reason.. disposing the client in the destructor throws an exception 
        //there is possibly a race condition on something that gets automatically collected via GC..
        //waiting feedback from AE.NET author..
        //~EmailAddress()
        //{
        //    if(_client.IsValueCreated && !_client.Value.IsDisposed)
        //        _client.Value.Dispose();
        //}

        public IEnumerable<EmailMessage> GetEmails(string to = null, string body = null, string subject = null)
        {
            var searchCondition = SearchCondition.Undeleted();
            if(to != null) searchCondition = searchCondition.And(SearchCondition.To(to));
            if(body != null) searchCondition = searchCondition.And(SearchCondition.Body(body));
            if(subject != null) searchCondition = searchCondition.And(SearchCondition.Subject(subject));

            using (var client = NewClient())
            {
                var msgs = client.SearchMessages(searchCondition);
                return msgs != null
                           ? msgs.Select(
                               x =>
                               AutoMapper.Mapper.Map<MailMessage, EmailMessage>(x.Value)).ToList()
                           : null;
            }
        }

        public ImapClient NewClient()
        {
            return new ImapClient(_host, _username, _password, ImapClient.AuthMethods.Login, _port, _isSsl);
        }
    }

    public class EmailMessage
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; }
    }
}
