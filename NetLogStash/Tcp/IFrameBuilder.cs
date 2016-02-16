using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Tcp
{
    public interface IFrameBuilder
    {
        byte[] EncodeFrame(byte[] payload, int offset, int count);
        bool TryDecodeFrame(byte[] buffer, int count, out int frameLength, out byte[] payload, out int payloadOffset, out int payloadCount);
    }
}
