using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Net.Client
{
	public sealed class NetClient : TcpClient
	{
		public event EventHandler OnConnectFailed;
		public event EventHandler OnConnected;
		public event EventHandler<NetworkStream> OnStreamOpened;

		public string Ip { get; set; }
		public int Port { get; set; }
		public NetworkStream Stream { get; set; }

		public NetClient(string ip, int port) =>
			(Ip, Port) = (ip, port);

		public async void RunConnect()
		{
			try
			{
				// Try to connect
				await ConnectAsync(Ip, Port);
				CallConnected(); // Call connected success event

				// Opens stream
				CallStreamOpened(Stream = GetStream());
			}
			catch
			{
				CallConnectFailed();
			}
		}

		private void CallConnectFailed() =>
			OnConnectFailed?.Invoke(this, EventArgs.Empty);

		private void CallConnected() =>
			OnConnected?.Invoke(this, EventArgs.Empty);

		private void CallStreamOpened(NetworkStream stream) =>
			OnStreamOpened?.Invoke(this, stream);
	}
}
