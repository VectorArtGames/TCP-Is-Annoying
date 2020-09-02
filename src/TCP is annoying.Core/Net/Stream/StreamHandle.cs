using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Memory;

namespace TCP_is_annoying.Core.Net.Stream
{
	public static class StreamHandle
	{
		public static async Task<DataMan> GetSize(this NetworkStream e)
		{
			var lenBuffer = new byte[sizeof(long)];
			await e.ReadAsync(lenBuffer, 0, lenBuffer.Length);
			var size = BitConverter.ToInt64(lenBuffer, 0);

			var dat = new byte[size];
			await e.ReadAsync(dat, 0, dat.Length);
			return new DataMan { Data = dat, Length = lenBuffer };
		}

		public static async void SendData(this NetworkStream e, object obj)
		{
			var dat = obj.Serialize();
			var len = new byte[dat.Length];
			BitConverter.GetBytes(dat.Length).CopyTo(len, 0);

			await e.WriteAsync(len, 0, len.Length);
			await e.WriteAsync(dat, 0, dat.Length);
			await e.FlushAsync();
		}
	}

	[Serializable]
	public struct DataMan
	{
		public byte[] Length;
		public byte[] Data;
	}
}
