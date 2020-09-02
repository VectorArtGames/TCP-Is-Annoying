using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Memory
{
	public static class BufferManager
	{
		public static byte[] CreateBuffer(byte[] data, long size)
		{
			var b = new byte[size];
			data.CopyTo(b, 0);

			return b;
		}
	}
}
