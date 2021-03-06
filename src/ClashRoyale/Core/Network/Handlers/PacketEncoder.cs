﻿using System.Threading.Tasks;
using ClashRoyale.Protocol;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace ClashRoyale.Core.Network.Handlers
{
    public class PacketEncoder : ChannelHandlerAdapter
    {
        public override Task WriteAsync(IChannelHandlerContext context, object msg)
        {
            if (!(msg is PiranhaMessage message)) return base.WriteAsync(context, null);

            message.Encode();
            message.Encrypt();

            var buffer = Unpooled.Buffer(7 + message.Length);

            buffer.WriteUnsignedShort(message.Id);
            buffer.WriteMedium(message.Length);
            buffer.WriteUnsignedShort(message.Version);

            buffer.WriteBytes(message.Writer);

            return base.WriteAsync(context, buffer);
        }
    }
}