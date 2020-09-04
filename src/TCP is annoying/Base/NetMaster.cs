using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Extensions;
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

		//! Server Code - Receive
		private static void Server_OnStreamOpened(object sender, NetworkStream e)
		{
			e.StartReading();
			//try
			//{
			//	var buffer = new byte[8];

			//	await e.ReadAsync(buffer, 0, 8);
			//	var len = BitConverter.ToInt32(buffer, 0);

			//	// Resize array of buffer
			//	Array.Resize(ref buffer, buffer.Length + len);

			//	await e.ReadAsync(buffer, 8, len);


			//	if (buffer.Length < 8) return;

			//	var data = buffer.Skip(8).Take(len).ToArray();

			//	var msg = Encoding.ASCII.GetString(data);

			//	Console.WriteLine($"Received Package!\n{len}\nData: {data.Length}\nMessage: {msg}");
			//}
			//catch
			//{
			//	Console.WriteLine("Malformed package!");
			//}
		}

		//! Client Code - Send
		private static void Client_OnStreamOpened(object sender, NetworkStream e)
		{
			for (var i = 0; i < 5; i++)
			{
				e.Send(Encoding.ASCII.GetBytes($"{i} Hello there =)"));
			}
		}

		private static void Server_OnNewConnection(object sender, TcpClient e)
		{
			Console.WriteLine("New client accepted!");
		}
	}
}
