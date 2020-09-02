using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Memory
{
	public static class ObjectConvert
	{
		public static byte[] Serialize<T>(this T obj)
		{
			if (obj == null) 
				return null;

			var f = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				f.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public static object Deserialize(this byte[] data)
		{
			if (data == null)
				return null;
			try
			{
				var f = new BinaryFormatter();
				using (var ms = new MemoryStream(data))
				{
					var obj = f.Deserialize(ms);
					return obj;
				}
			}
			catch
			{
				return null;
			}
		}
	}
}
