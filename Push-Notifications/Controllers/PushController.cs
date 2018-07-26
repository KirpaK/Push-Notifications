using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Push_Notifications.Models;
using WebPush;

namespace Push_Notifications.Controllers
{
    public class PushController : Controller
    {
        static PushSubscription pushSubscription;
        WebPushClient pusher;

        public PushController()
        {
            pusher = new WebPushClient();
        } 

        public IActionResult Test()
        {
            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                title = "Welcome Test",
                body = "Thank you for enabling push notifications",
                icon = "/android-chrome-192x192.png"
            });
            var subject = @"mailto:kirillkirpa@gmail.com";
            var publicKey = @"...";
            var privateKey = @"....";
            var vapid = new VapidDetails(subject, publicKey, privateKey);
            var t = Task.Run(() => Console.Write("@@@@@"));
            t.Wait(10000);
            pusher.SendNotification(pushSubscription, payload, vapid);

            return Ok();
        }

        [HttpPost]
        public IActionResult Subscribe([FromBody] Subscription subscription)
        {
            pushSubscription = new PushSubscription(subscription.Endpoint, subscription.Keys.P256dh, subscription.Keys.Auth);

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                title = "Welcome",
                body = "Thank you for enabling push notifications",
                icon = "/android-chrome-192x192.png"
            });
            var options = new { TTL = 3600 };
            var subject = @"mailto:kirillkirpa@gmail.com";
            var publicKey = @"...";
            var privateKey = @"...";
            var vapid = new VapidDetails(subject, publicKey, privateKey);
            pusher.SendNotification(pushSubscription, payload, vapid);
            return Ok();
        }
    }
}