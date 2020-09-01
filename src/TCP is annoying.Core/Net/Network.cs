using System;
using System.Net.Sockets;
using TCP_is_annoying.Core.Net.Client;
using TCP_is_annoying.Core.Net.Server;

namespace TCP_is_annoying.Core.Net
{
	public class Network
	{
		public event EventHandler OnInitializedServer;
		public event EventHandler OnInitializedClient;

		public NetClient Client { get; set; }
		public NetServer Server { get; set; }

		public void InitializeServer()
		{
			Server = new NetServer(11555);

			if (Server == null) return;
			CallInitializedServer();
		}

		public void InitializeClient(string ip, int port)
		{
			Client = new NetClient(ip, port);

			if (Client == null) return;
			CallInitializedClient();
		}

		private void CallInitializedServer() =>
			OnInitializedServer?.Invoke(this, EventArgs.Empty);

		private void CallInitializedClient() =>
			OnInitializedClient?.Invoke(this, EventArgs.Empty);
	}
}
