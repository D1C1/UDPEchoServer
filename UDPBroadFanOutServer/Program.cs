using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UDPBroadFanOutServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Creates a UdpClient for reading incoming data.
            UdpClient udpServer = new UdpClient(7065);

            //Creates an IPEndPoint to record the IP Address and port number of the sender.  
            //IPAddress ip = IPAddress.Parse("192.168.103.148");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7065);
            //IPEndPoint object will allow us to read datagrams sent from another source.

            try
            {
                // Blocks until a message is received on this socket from a remote host (a client).
                Console.WriteLine("Server is listening");

                Byte[] receiveBytes = udpServer.Receive(ref RemoteIpEndPoint);
                //Server is now activated");

                string receivedData = Encoding.ASCII.GetString(receiveBytes);
                string[] data = receivedData.Split(' ');
                //string text = data[1] + data[2];

                Console.WriteLine(receivedData);
                //Console.WriteLine("Received from: " + clientName.ToString() + " " + text.ToString());
                Userstories recData = JsonConvert.DeserializeObject<Userstories>(receivedData);
                Console.WriteLine($"{recData.Id}  {recData.Title} temp is {recData.Description}");

                Console.WriteLine("This message was sent from " +
                                  RemoteIpEndPoint.Address.ToString() +
                                  " on their port number " +
                                  RemoteIpEndPoint.Port.ToString());
                await Post<Userstories, Userstories>("https://proeverest.azurewebsites.net/api/UserStories", recData);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static async Task<TOut> Post<TIn, TOut>(string uri, TIn item)
        {
            using HttpClient client = new HttpClient();
            string jsonStr = JsonConvert.SerializeObject(item);
            StringContent requestContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) // all 2xx status codes
            {
                //Console.WriteLine(jsonString);
                TOut data = JsonConvert.DeserializeObject<TOut>(responseContent);
                return data;
            }
            throw new KeyNotFoundException($"Status code={response.StatusCode} Message={responseContent}");
        }

        public class Userstories
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int BussinessValue { get; set; }
            public string State { get; set; }
        }
    }
}
