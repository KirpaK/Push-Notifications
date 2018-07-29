using System;
using System.Collections.Generic;
using System.Text;

namespace DataLevelLogic.Models
{
    public class Device
    {
        public Guid ID { get; set; }
        public string Endpoint { get; set; } 
        public string P256dh { get; set; }
        public string Auth { get; set; }
        public DateTimeOffset LastSync { get; set; }
    }
}
