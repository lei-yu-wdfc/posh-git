using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AE.Net.Mail;

namespace Wonga.QA.Framework.Email
{
    internal static class EmailMappings
    {
        private static readonly object _locker = new object();
        private static bool _configured = false;

        public static void Configure()
        {
            lock (_locker)
            {
                if (_configured)
                    return;
                AutoMapper.Mapper.CreateMap<MailMessage, EmailMessage>().ConstructUsing(
                    x => new EmailMessage()
                             {
                                 Body = x.Body,
                                 From = x.From.User,
                                 To = x.To.Select(toAddress => toAddress.Address),
                                 Subject = x.Subject
                             });

                _configured = true;
            }
        }
    }
}
