using System;
using System.IO;
using System.Net;
using System.Text;
using Gallio.Framework;
using Gallio.Model;
using OpenQA.Selenium.Remote;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI
{
    public static class SauceRestClient
    {
        
        private static string Username()
        {
            return Config.Ui.RemoteUsername;
        }

        private static string AccessKey()
        {
            return Config.Ui.RemoteApiKey;
        }

        private const string Url = "http://saucelabs.com/rest/v1/{0}/jobs/{1}";


        public static string JobResultJsonString()
        {
            switch (TestContext.CurrentContext.Outcome.Status)
            {
                case (TestStatus.Passed):
                    return "{\"passed\": true}";
                case (TestStatus.Failed):
                case (TestStatus.Inconclusive):
                    return "{\"passed\": false}";
                default:
                    throw new NotSupportedException(String.Format("Test outcome was: {0}", TestContext.CurrentContext.Outcome.Status));
            }
        }


        public static void UpdateJobPassFailStatus(SessionId jobId)
        {
            var username = Username();
            var accessKey = AccessKey();

            //build and send json request
            var jsonString = JobResultJsonString();
            byte[] jsonStringToBytes = Encoding.UTF8.GetBytes(jsonString);
            var uri = new Uri(string.Format(Url, username, jobId));
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var auth = EncodeTo64(string.Format("{0}:{1}", username, accessKey));
            request.Headers.Add("Authorization", String.Format("Basic {0}", auth));
            request.ContentLength = jsonStringToBytes.Length;
            request.Method = "PUT";
            request.ContentType = "application/json";
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(jsonStringToBytes, 0, jsonStringToBytes.Length);
            }
            
            //assert response from server is OK
            var response = (HttpWebResponse)request.GetResponse();
            if (!"OK".Equals(response.StatusCode.ToString()))
            {
                throw new Exception(string.Format("Response from Sauce API was not as expected. Expected \"OK\" but was \"{0}\"", response.StatusCode));
            }

        }

        public static string EncodeTo64(string toEncode)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            var returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

    }
}
