using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SequenceNumberManager : ISequenceNumberProvider
    {
        private Dictionary<byte, ushort> Table = new Dictionary<byte, ushort>();

        public ushort GetValue(byte dataId)
        {
            return Table.ContainsKey(dataId) ? Table[dataId] : (ushort)0;
        }

        public void SetValue(byte dataId, ushort value)
        {
            Table[dataId] = value;
        }

        public void ClearValue(byte dataId)
        {
            Table.Remove(dataId);
        }

        public ushort IncrementValue(byte dataId)
        {
            return Table.ContainsKey(dataId) ? ++Table[dataId] : Table[dataId] = 0;
        }

        public bool UpdateNewer(byte dataId, ushort value)
        {
            if (Table.ContainsKey(dataId) && (short)(value - Table[dataId]) < 0) return false;
            Table[dataId] = value;
            return true;
        }

        public bool UpdateConsecutive(byte dataId, ushort value)
        {
            if (Table.ContainsKey(dataId) && Table[dataId] + 1 != value) return false;
            Table[dataId] = value;
            return true;
        }
    }
}