﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace UDPBroadFanOutServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creates a UdpClient for reading incoming data.
            UdpClient udpServer = new UdpClient(7000);

            //Creates an IPEndPoint to record the IP Address and port number of the sender.  
            //IPAddress ip = IPAddress.Parse("192.168.103.148");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7000);
            //IPEndPoint object will allow us to read datagrams sent from another source.

            try
            {
                // Blocks until a message is received on this socket from a remote host (a client).
                Console.WriteLine("Server is listening");

                Byte[] receiveBytes = udpServer.Receive(ref RemoteIpEndPoint);
                //Server is now activated");

                string receivedData = Encoding.ASCII.GetString(receiveBytes);
                string[] data = receivedData.Split(' ');
                string clientName = data[0];
                //string text = data[1] + data[2];

                Console.WriteLine(receivedData);
                //Console.WriteLine("Received from: " + clientName.ToString() + " " + text.ToString());
                SensorDataModel recData = JsonConvert.DeserializeObject<SensorDataModel>(receivedData);
                Console.WriteLine($"{recData.RoomId} temp is {recData.Temperature}");
                Console.WriteLine("This message was sent from " +
                                  RemoteIpEndPoint.Address.ToString() +
                                  " on their port number " +
                                  RemoteIpEndPoint.Port.ToString());

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://apimentalshowerindoor.azurewebsites.net/api/IndoorClimate");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(recData);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
