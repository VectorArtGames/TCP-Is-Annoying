﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_is_annoying.Core.Messages
{
    [Serializable]
    public struct Message
    {
		public const long BufferSize = 256;

        public byte[] Data;
    }
}
