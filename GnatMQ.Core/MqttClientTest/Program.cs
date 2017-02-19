using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttClientTest
{
    class Program
    {
        static void Main(string[] args)
        {// create client instance
            MqttClient client = new MqttClient(IPAddress.Parse("127.0.0.1"));

            // register to message received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/sample/test" with QoS 2
            client.Subscribe(new string[] { "/sample/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            int count = 0;
            while (true)
            {
                client.Publish("/sample/test", Encoding.UTF8.GetBytes($"kirim {count++}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,false); 
 
                Thread.Sleep(100);

            }
 
        }


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(e.Topic + ":" + System.Text.Encoding.UTF8.GetString(e.Message));
        }
    }
}
