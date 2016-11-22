using RED.Interfaces.Network;
using System.Collections.Generic;

namespace RED.ViewModels.Network
{
    public class SequenceNumberManager : ISequenceNumberProvider
    {
        private Dictionary<ushort, ushort> Table = new Dictionary<ushort, ushort>();

        public ushort GetValue(ushort dataId)
        {
            return Table.ContainsKey(dataId) ? Table[dataId] : (ushort)0;
        }

        public void SetValue(ushort dataId, ushort value)
        {
            Table[dataId] = value;
        }

        public void ClearValue(ushort dataId)
        {
            Table.Remove(dataId);
        }

        public ushort IncrementValue(ushort dataId)
        {
            return Table.ContainsKey(dataId) ? ++Table[dataId] : Table[dataId] = 0;
        }

        public bool UpdateNewer(ushort dataId, ushort value)
        {
            if (Table.ContainsKey(dataId) && (short)(value - Table[dataId]) < 0) return false;
            Table[dataId] = value;
            return true;
        }

        public bool UpdateConsecutive(ushort dataId, ushort value)
        {
            if (Table.ContainsKey(dataId) && Table[dataId] + 1 != value) return false;
            Table[dataId] = value;
            return true;
        }
    }
}