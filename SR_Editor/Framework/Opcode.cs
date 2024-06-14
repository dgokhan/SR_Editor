using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.Framework.Opcode
{
    public struct General
    {
        public const ushort HANDSHAKE = 0x5000;
        public const ushort HANDSHAKE_ACCEPT = 0x9000;
        public const ushort IDENTITY = 0x2001;
        public const ushort SEED_1 = 0x2005;
        public const ushort SEED_2 = 0x6005;
        public const ushort PING = 0x2002;
    }

    public struct Global
    {
        public const ushort CHECKVERSION = 0x6000;

        public struct Request
        {
            public const ushort LOGIN = 0x7001;
        }

        public struct Response
        {
            public const ushort PATCH = 0xA100;
        }
    }

    public struct Custom
    {
        public const ushort CHECKVERSION = 0x6000;

        public struct Request
        {
            public const ushort PATCH = 0x6100;
        }

        public struct Response
        {
            public const ushort PATCH = 0xA100;
        }
    }
}
