using DataLevelLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPush;

namespace Push_Notifications.Models
{
    public class Subscription
    {
        public string Endpoint { get; set; }
        public SubscriptionKeys Keys { get; set; } = new SubscriptionKeys();

        public Subscription(Device device = null)
        {
            if(device != null)
            {
                Endpoint = device.Endpoint;
                Keys.Auth = device.Auth;
                Keys.P256dh = device.P256dh; 
            }
        }

        public Device GetDevice() => new Device {
            Endpoint = Endpoint,
            Auth = Keys.Auth,
            P256dh = Keys.P256dh
        };

        public PushSubscription GetPushSubscription() => new PushSubscription(Endpoint, Keys.P256dh, Keys.Auth);


        public class SubscriptionKeys
        {
            public string P256dh { get; set; }
            public string Auth { get; set; }
        }
    }
}
