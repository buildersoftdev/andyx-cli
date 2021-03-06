using Andy.X.Cli.Models;
using Andy.X.Cli.Utilities.Extensions;
using ConsoleTables;
using Newtonsoft.Json;

namespace Andy.X.Cli.Services
{
    public static class ProducerService
    {
        public static void GetProducers()
        {
            var node = NodeService.GetNode();

            string request = $"{node.NodeUrl}api/v1/producers";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-called-by", $"Andy X Cli");
                client.AddBasicAuthorizationHeader(node);

                HttpResponseMessage httpResponseMessage = client.GetAsync(request).Result;
                string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var table = new ConsoleTable("ID", "PRODUCER_ID");
                    //List<string> list = content.JsonToObject<List<string>>();
                    List<string> list = JsonConvert.DeserializeObject<List<string>>(content);

                    int k = 0;
                    foreach (var item in list)
                    {
                        k++;
                        table.AddRow(k, item);
                    }
                    table.Write();
                }
            }
            catch (Exception)
            {
                var table = new ConsoleTable("STATUS", "ERROR");

                table.AddRow("NOT_CONNECTED", "It can not connect to the node, check network connectivity");
                table.Write();
            }

        }

        public static void GetProducer(string producer)
        {
            var node = NodeService.GetNode();

            string request = $"{node.NodeUrl}api/v1/producers/{producer}";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-called-by", $"Andy X Cli");
                client.AddBasicAuthorizationHeader(node);

                HttpResponseMessage httpResponseMessage = client.GetAsync(request).Result;
                string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var table = new ConsoleTable("TENANT", "PRODUCT", "COMPONENT", "TOPIC", "ID", "PRODUCER_NAME", "IS_LOCAL");
                    var producerDetail = JsonConvert.DeserializeObject<Producer>(content);
                    table.AddRow(producerDetail.Tenant, producerDetail.Product, producerDetail.Component, producerDetail.Topic, producerDetail.Id, producerDetail.ProducerName, producerDetail.IsLocal);
                    table.Write();
                }
                else
                {
                    var table = new ConsoleTable("STATUS", "ERROR");

                    table.AddRow(httpResponseMessage.StatusCode, content);
                    table.Write();
                }
            }
            catch (Exception)
            {
                var table = new ConsoleTable("STATUS", "ERROR");

                table.AddRow("NOT_CONNECTED", "It can not connect to the node, check network connectivity");
                table.Write();
            }
        }

        public static void GetProducerStats(string producer)
        {
            var node = NodeService.GetNode();

            string request = $"{node.NodeUrl}api/v1/producers/{producer}";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-called-by", $"Andy X Cli");
                client.AddBasicAuthorizationHeader(node);

                bool stop = false;
                Console.CancelKeyPress += delegate
                {
                    stop = true;
                };

                while (true)
                {
                    Thread.Sleep(1000);
                    HttpResponseMessage httpResponseMessage = client.GetAsync(request).Result;
                    string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.Clear();
                        var table = new ConsoleTable("TIME", "TENANT", "PRODUCT", "COMPONENT", "TOPIC", "PRODUCER_NAME", "MESSAGE_PRODUCED");
                        var producerDetail = JsonConvert.DeserializeObject<Producer>(content);
                        table.AddRow(DateTime.Now.ToString("HH:mm:ss"), producerDetail.Tenant, producerDetail.Product, producerDetail.Component, producerDetail.Topic, producerDetail.ProducerName, producerDetail.CountMessagesProducedSinceConnected);
                        table.Write();
                    }
                    else
                    {
                        var table = new ConsoleTable("STATUS", "ERROR");

                        table.AddRow(httpResponseMessage.StatusCode, content);
                        table.Write();
                        break;
                    }

                    if (stop == true)
                        break;
                }

            }
            catch (Exception)
            {
                var table = new ConsoleTable("STATUS", "ERROR");

                table.AddRow("NOT_CONNECTED", "It can not connect to the node, check network connectivity");
                table.Write();
            }
        }


    }
}
