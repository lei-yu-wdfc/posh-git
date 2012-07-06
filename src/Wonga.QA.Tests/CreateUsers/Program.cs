using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MbUnit.Framework;
using NHamcrest.Core;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;

namespace CreateUsers
{

    class Program
    {
        public static int AvailableCustomerCount;
        public static int NumberOfUsersToCreate;
        public const int Threshold = 1000;

        static void Main()
        {
            
            var helper = VanillaCustomerHelper.New().WithEnvironment(Config.AUT, Config.SUT);
            AvailableCustomerCount = helper.Count();
            helper.DeleteAllUsedUsers();
            
            //AvailableCustomerCount = 1;
            NumberOfUsersToCreate = Threshold - AvailableCustomerCount;
            if (NumberOfUsersToCreate > 0)
                CreateUsersInParallel();

            Console.WriteLine("We good");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void Notify(int exceptionCount, int toCreate, int created)
        {
            string recipient = "ben.ifie@wonga.com";
            string subject = "Users Not Created";
            string body = 
                String.Format("Test Creation application has stopped running on {0}/{1}.\n\nThere were {2} exceptions encountered. View Logs.\n\n Out of {3} users, only {4} were created",
                Config.AUT, Config.SUT, exceptionCount, toCreate, created);
            SendMail(recipient, subject, body);
        }

        public static void SendMail(string messageRecepient, string messageSubject, string messageBody)
        {
            const string senderEmail = "jaminonline@gmail.com";

            var to = new MailAddress(messageRecepient);
            var from = new MailAddress(senderEmail);
            var message = new MailMessage(from, to);
            message.Subject = messageSubject;
            message.Body = messageBody;
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential(senderEmail, "easyPea5y");
                client.EnableSsl = true;
                Console.WriteLine("Sending an e-mail message to {0} at {1} by using the SMTP host={2}.",
                to.User, to.Host, client.Host);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught whilst Sending Email Message: {0}", ex);
                }
            }
            
        }


        public static void Create(int thread)
        {
            if (NumberOfUsersToCreate <= 0)
                return;
            var c = new VanillaCustomer();
            c.InsertUserToDb();
            Console.WriteLine(String.Format("User number {0} with email {1} successfully created by thread Id:{2}", thread.ToString(CultureInfo.InvariantCulture), c.Email, Thread.CurrentThread.ManagedThreadId));
            AvailableCustomerCount++;
            NumberOfUsersToCreate--;

            //Thread.Sleep(500);
            //if (Thread.CurrentThread.ManagedThreadId.Equals(9))
            //    throw new Exception("noooooooo");
            //Console.WriteLine(String.Format("User number {0} with email bla.com successfully created {1}", thread.ToString(CultureInfo.InvariantCulture), Thread.CurrentThread.ManagedThreadId));
            //NumberOfUsersToCreate--;
        }

        private static void CreateUsersInParallel()
        {
            int intendedNumberOfUsersToCreate = NumberOfUsersToCreate;
            
            // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
            var exceptions = new ConcurrentQueue<Exception>();
            var po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var cts = new CancellationTokenSource();
            po.CancellationToken = cts.Token;

            // Run a task so that we can cancel Parallel.ForEach Loop from another thread if too many exceptions.
            Task.Factory.StartNew(() =>
            {
                while (exceptions.Count < 10)
                {
                    //do nothing
                }
                cts.Cancel();
                int created = intendedNumberOfUsersToCreate - NumberOfUsersToCreate;
                Notify(exceptions.Count, intendedNumberOfUsersToCreate, created);
            });
            
            Console.WriteLine("No of users to create is {0}", NumberOfUsersToCreate);
            var amountToCreate = Enumerable.Range(0, NumberOfUsersToCreate).ToArray();
            try
            {
                Parallel.ForEach(amountToCreate, po, toCreate =>
                {
                    try
                    {
                        Create(toCreate);
                        po.CancellationToken.ThrowIfCancellationRequested();
                    }
                    // Store the exception and continue with the loop.
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
