
using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {


        static Timer timer;

        static void Main(string[] args)
        {
            timer = new Timer(async (sender) =>
            {
                await GetMessages();
            }, null, 0, 5000);

            Console.Read();
        }


        
        /// <summary>
        /// Get Messages from Service Bus Subscription on the given Topic
        /// </summary>
        /// <returns></returns>
        static async Task GetMessages()
        {
            try
            {
                Console.WriteLine("Polling for new messages");
                HttpClient httpClient = new HttpClient();

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ServiceBusConfig.resourceUri + $"/subscriptions/{ServiceBusConfig.subscriptionName}/messages/head"),
                    Method = HttpMethod.Delete
                };

                httpRequestMessage.Headers.Add("Authorization", ServiceBusConfig.GetToken());
                var result = await httpClient.SendAsync(httpRequestMessage);

                var content = (StreamContent)result.Content;
                ServiceBusMessage sbMessage = JsonConvert.DeserializeObject<ServiceBusMessage>(result.Content.ReadAsStringAsync().Result);
                Console.WriteLine(sbMessage.UserProperties.Guid);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

            }
        }
    }
}
