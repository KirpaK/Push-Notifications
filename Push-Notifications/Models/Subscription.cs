using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Push_Notifications.Models
{
    public class Subscription
    {
        public string Endpoint { get; set; }
        public SubscriptionKeys Keys { get; set; }

        public class SubscriptionKeys
        {
            public string P256dh { get; set; }
            public string Auth { get; set; }
        }
    }
}
