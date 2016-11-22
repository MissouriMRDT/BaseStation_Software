using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces.Network
{
    public interface ISequenceNumberProvider
    {
        ushort GetValue(ushort dataId);
        void SetValue(ushort dataId, ushort value);
        void ClearValue(ushort dataId);
        ushort IncrementValue(ushort dataId);
        bool UpdateNewer(ushort dataId, ushort value);
        bool UpdateConsecutive(ushort dataId, ushort value);
    }
}