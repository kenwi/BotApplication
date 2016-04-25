using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Bot.Connector.DirectLine.Models;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var client = new LocalhostConnectorClient("YourAppId", "YourAppSecret");
        
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var result = await client.HttpClient.PostAsync(LocalhostConnectorClient.baseUri.AbsoluteUri, null);

                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsAsync(typeof(Conversation)) as Conversation;
                    Console.WriteLine(response.ConversationId);
                }
            }).Wait();            
        }
    }

    public class LocalhostConnectorClient : ConnectorClient
    {
        public static Uri baseUri = new Uri("http://localhost:3978/api/conversations");

        public LocalhostConnectorClient(string appId, string appSecret) : base(baseUri, new LocalhostCredentials(appId, appSecret))
        {
            this.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", appId);

            var byteArray = Encoding.ASCII.GetBytes($"{appId}:{appSecret}");
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }

    public class LocalhostCredentials : ConnectorClientCredentials
    {
        public LocalhostCredentials(string appId, string appSecret) : base(appId, appSecret)
        {
            this.Endpoint = "http://localhost:3978/api/conversations";
        }
    }
}
