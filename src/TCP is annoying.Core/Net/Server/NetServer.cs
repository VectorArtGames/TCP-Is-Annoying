using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Net.Server
{
	public sealed class NetServer : TcpListener
	{
		public event EventHandler OnStarted;

		public string IP =>
			LocalEndpoint.ToString().Split(':')[0];

		public int Port { get; set; }

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
				do // Loop through pending connection requests
				{
					Debug.WriteLine("Accepting ..");
				} while (Pending());

				await Task.Delay(100);
			}
		}

		private void CallStarted() =>
			OnStarted?.Invoke(this, EventArgs.Empty);
	}
}
