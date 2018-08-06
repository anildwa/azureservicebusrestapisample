using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {

        static int msgCount = 100;

        static void Main(string[] args)
        {


            MainAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Main Async method to post messages to Service Bus Topic
        /// </summary>
        /// <returns></returns>
        static async Task MainAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                for(int i = 0; i<= msgCount; i++)
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(ServiceBusConfig.resourceUri + "/messages"),
                        Method = HttpMethod.Post
                    };
                    httpRequestMessage.Headers.Add("Authorization", ServiceBusConfig.GetToken());

                    var msg = BuildServiceBusMessage();
                    msg[0].UserProperties.Guid = Guid.NewGuid().ToString();
                    var sbmessage = JsonConvert.SerializeObject(msg);
                    httpRequestMessage.Content = new StringContent(sbmessage);
                    httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.microsoft.servicebus.yml");
                    var result = await httpClient.SendAsync(httpRequestMessage);

                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        Console.WriteLine($"sending msg no {i}: {msg[0].UserProperties.Guid}");
                    }
                }

            }
            catch (Exception ex)
            {

                
            }

        }

        /// <summary>
        /// Build Service Bus Message
        /// </summary>
        /// <returns></returns>
        static ServiceBusMessage[] BuildServiceBusMessage()
        {
            var brokerProperties = new BrokerProperties() { Label = "TestLabel", TimeToLiveTimeSpan = TimeSpan.FromSeconds(120).ToString() };
            var userProperties = new UserProperties() { CustomerName = "ABC", Priority = "Medium" };

            var sbMessage = new ServiceBusMessage() { Body = "Test Message 1", BrokerProperties = brokerProperties, UserProperties = userProperties };

            var messages = new List<ServiceBusMessage>();
            messages.Add(sbMessage);
            return messages.ToArray();

        }



    }



    
}
