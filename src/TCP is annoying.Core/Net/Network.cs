using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Net.Server;

namespace TCP_is_annoying.Core.Net
{
	public class Network
	{
		public event EventHandler OnInitialized;

		public TcpClient Client { get; set; }
		public NetServer Server { get; set; }

		public void Initialize()
		{
			Server = new NetServer(11555);

			if (Server == null) return;
			CallInitialized();
		}

		private void CallInitialized() =>
			OnInitialized?.Invoke(this, EventArgs.Empty);
	}
}
