using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ServiceBusConfig
    {

        public static string topicName = "topic1";
        public static string subscriptionName = "sub1";
        public static string serviceBusEndpoint = "<service bus namespace>.servicebus.windows.net";
        public static string serviceBusServiceName = "<service bus namespace>";
        public static string sasKey = "<service bus access key>";
        public static string resourceUri = $"https://{serviceBusEndpoint}/{topicName}";
        public static string keyName = "RootManageSharedAccessKey";



        public static string GetToken()
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + 3600); //EXPIRES in 1h 
            string stringToSign = WebUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(sasKey));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture,
            "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                WebUtility.UrlEncode(resourceUri), WebUtility.UrlEncode(signature), expiry, keyName);

            return sasToken;
        }

    }


    public class ServiceBusMessage
    {
        public string Body { get; set; }
        public BrokerProperties BrokerProperties { get; set; }
        public UserProperties UserProperties { get; set; }
    }


    public class BrokerProperties
    {
        public string Label { get; set; }
        public string TimeToLiveTimeSpan { get; set; }
    }


    public class UserProperties
    {
        public string Priority { get; set; }
        public string CustomerName { get; set; }
        public string Guid { get; set; }

    }

}
