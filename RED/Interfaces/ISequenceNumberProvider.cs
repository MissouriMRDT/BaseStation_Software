using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface ISequenceNumberProvider
    {
        ushort GetValue(byte dataId);
        void SetValue(byte dataId, ushort value);
        void ClearValue(byte dataId);
        ushort IncrementValue(byte dataId);
        bool UpdateNewer(byte dataId, ushort value);
        bool UpdateConsecutive(byte dataId, ushort value);
    }
}