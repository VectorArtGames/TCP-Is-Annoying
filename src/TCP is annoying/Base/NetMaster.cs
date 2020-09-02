using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Memory;
using TCP_is_annoying.Core.Messages;
using TCP_is_annoying.Core.Net;

namespace TCP_is_annoying.Base
{
    public class NetMaster
    {
        public static void Run()
        {
            var n = new Network();
            n.OnInitializedClient += Net_OnInitializedClient;
			n.OnInitializedServer += N_OnInitializedServer;

			n.InitializeServer();
        }

		private static void N_OnInitializedServer(object sender, EventArgs e)
		{
			if (!(sender is Network net)) return;

			Console.WriteLine($"Listening at: {net.Server.LocalEndpoint}");
			net.Server.Start();

			net.InitializeClient("127.0.0.1", net.Server.Port);
		}

		private static void Net_OnInitializedClient(object sender, EventArgs e)
		{
			if (!(sender is Network net)) return;
			net.Client.OnConnected += (s, g) =>
			{
				Console.WriteLine("Connected!");
			};

			net.Client.OnConnectFailed += (s, g) =>
			{
				Console.WriteLine("Connection Failed ..");
			};

			net.Client.OnStreamOpened += Client_OnStreamOpened;

			net.Server.OnNewConnection += Server_OnNewConnection;
			net.Server.OnStreamOpened += Server_OnStreamOpened;
			Console.WriteLine("Client initialized ..");
			net.Client.RunConnect();
		}

		private static void Server_OnStreamOpened(object sender, NetworkStream e)
		{
			var m = new byte[Message.BufferSize];
			e.Read(m, 0, m.Length);

			if (m.Deserialize<Message>() is Message msg)
			{
				Console.WriteLine(Encoding.ASCII.GetString(msg.Data));
			}
		}

		private static void Client_OnStreamOpened(object sender, NetworkStream e)
		{
			var m = new byte[Message.BufferSize];

			var msg = new Message
			{
				Data = Encoding.ASCII.GetBytes("Hello father, I've missed you..\nI'm your long lost son.\n")
			};

			if (msg.Data.Length > m.Length)
			{
				Console.WriteLine("Data too long!\nPlease increase buffer size");
				return;
			}

			var b = msg.Serialize();

			b.CopyTo(m, 0);

			e.Write(m, 0, m.Length);
			e.Flush();
		}

		private static void Server_OnNewConnection(object sender, TcpClient e)
		{
			Console.WriteLine("New client accepted!");
		}
	}
}
