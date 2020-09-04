using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Extensions
{
	public static class StreamExtension
	{
		public static AutoResetEvent ReadReset = new AutoResetEvent(false);

		/// <summary>
		/// Send Package to NetworkStream
		/// </summary>
		/// <param name="e">NetworkStream</param>
		/// <param name="package">Package of Bytes</param>
		public static async Task<bool> Send(this NetworkStream e, byte[] package)
		{
			try
			{
				var l = new byte[sizeof(long)];
				var msg = package;

				// Get size of package - Add it to length buffer
				BitConverter.GetBytes(msg.Length)
					.CopyTo(l, 0);


				// Send length buffer to stream
				await e.WriteAsync(l, 0, l.Length);

				// Send Package buffer to stream
				await e.WriteAsync(msg, 0, msg.Length);

				// Flush stream
				await e.FlushAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static async void StartReading(this NetworkStream e)
		{
			try
			{
				while (e.CanRead)
				{
					while (e.DataAvailable)
					{
						Debug.WriteLine("Reading ..");
						var buffer = new byte[8];

						await e.ReadAsync(buffer, 0, 8);
						var len = BitConverter.ToInt32(buffer, 0);

						// Resize array of buffer
						Array.Resize(ref buffer, buffer.Length + len);

						await e.ReadAsync(buffer, 8, len);


						if (buffer.Length < 8) return;

						var data = buffer.Skip(8).Take(len).ToArray();

						var msg = Encoding.ASCII.GetString(data);

						Console.WriteLine($"Received Package!\n{len}\nData: {data.Length}\nMessage: {msg}");
						
						ReadReset.Set();
					}
					ReadReset.WaitOne(-1);
				}
			}
			catch
			{
				Console.WriteLine("Malformed package!");
			}
		}
	}
}
