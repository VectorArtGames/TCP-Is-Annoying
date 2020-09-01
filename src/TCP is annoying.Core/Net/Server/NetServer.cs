using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Net.Server
{
	public sealed class NetServer : TcpListener
	{
		public event EventHandler OnStarted;
		public event EventHandler<TcpClient> OnNewConnection;
		public event EventHandler<NetworkStream> OnStreamOpened;

		public int Port { get; set; }
		public NetworkStream Stream { get; set; }

		public NetServer(int port) : base(IPAddress.Any, port) =>
			Port = port;

		public new void Start()
		{
			base.Start();
			if (!Active) return;
			ListenConnections();
			CallStarted();
		}

		private async void ListenConnections()
		{
			while (true)
			{
				while (Pending())
				{
					await AcceptTcpClientAsync()
						.ContinueWith((t) =>
						{
							var client = t.Result;
							CallNewConnection(client);

							// Opens stream
							CallStreamOpened(Stream = client.GetStream());
						});
				}

				await Task.Delay(100);
			}
		}

		private void CallStarted() =>
			OnStarted?.Invoke(this, EventArgs.Empty);

		private void CallNewConnection(TcpClient client) =>
			OnNewConnection?.Invoke(this, client);

		private void CallStreamOpened(NetworkStream stream) =>
			OnStreamOpened?.Invoke(this, stream);
	}
}
