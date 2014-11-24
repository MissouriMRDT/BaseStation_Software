using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Contexts
{
    public class ErrorMetadataContext : IMetadata
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Subsystem { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string Units { get; set; }
    }
}
