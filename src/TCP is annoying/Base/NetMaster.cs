using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Memory;
using TCP_is_annoying.Core.Messages;
using TCP_is_annoying.Core.Net;
using TCP_is_annoying.Core.Net.Stream;

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

		//! Server Code - Receive
		private static async void Server_OnStreamOpened(object sender, NetworkStream e)
		{
			var buffer = new byte[63635];

			await e.ReadAsync(buffer, 0, 8);
			var len = BitConverter.ToInt32(buffer.Take(8).ToArray(), 0);

			await e.ReadAsync(buffer, 8, len);

			var data = buffer.Skip(8).Take(len).ToArray();

			var msg = Encoding.ASCII.GetString(data);

			Console.WriteLine($"Received! {len}\nData: {data.Length}\nMessage: {msg}");
		}

		//! Client Code - Send
		private static async void Client_OnStreamOpened(object sender, NetworkStream e)
		{
			var l = new byte[sizeof(long)];

			var msg = Encoding.ASCII.GetBytes(new string('>', 1555));

			BitConverter.GetBytes(msg.Length)
				.CopyTo(l, 0);

			await e.WriteAsync(l, 0, l.Length);
			await e.WriteAsync(msg, 0, msg.Length);
			await e.FlushAsync();
		}

		private static void Server_OnNewConnection(object sender, TcpClient e)
		{
			Console.WriteLine("New client accepted!");
		}
	}
}
