using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
			var m = new byte[255];
			while (e.Read(m, 0, m.Length) > 0)
			{
				Console.WriteLine(BitConverter.ToInt32(m, 0));
			}
		}

		private static void Client_OnStreamOpened(object sender, NetworkStream e)
		{
			var m = new byte[255];

			BitConverter
				.GetBytes(55)
				.CopyTo(m, 0);

			e.Write(m, 0, m.Length);
			e.Flush();
		}

		private static void Server_OnNewConnection(object sender, TcpClient e)
		{
			Console.WriteLine("New client accepted!");
		}
	}
}
