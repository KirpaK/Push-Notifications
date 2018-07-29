using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Push_Notifications.DataLevelLogic;
using Push_Notifications.Models;
using WebPush;

namespace Push_Notifications.Controllers
{
    [Route("api/[controller]")]
    public class PushController : Controller
    {
        private WebPushClient pusher;
        private PushContext db;
        private Settings settings;

        public PushController(PushContext db, IOptions<Settings> settings)
        {
            this.pusher = new WebPushClient();
            this.db = db;
            this.settings = settings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Notify([FromBody]PushPayload payload)
        {
            var subscription = new Subscription(db.Devices.Find(payload.ID));

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                title = "Welcome Test",
                body = $"Thank you for enabling {payload.ID} push notifications",
                icon = "/android-chrome-192x192.png"
            });
            var subject = @"mailto:kirillkirpa@gmail.com";
            var publicKey = settings.VAPIDPublicKey;
            var privateKey = settings.VAPIDPrivateKey;
            var vapid = new VapidDetails(subject, publicKey, privateKey);
            if (payload.withPause)
                Thread.Sleep(5000);
            await pusher.SendNotificationAsync(subscription.GetPushSubscription(), data, vapid);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Devices(int startDateIndex = 0)
        {
            try
            {
                var take = 10;
                if (startDateIndex < 0) startDateIndex = 0;
                var devices = await db.Devices.AsNoTracking().Skip(startDateIndex * take).Take(take).ToListAsync();
                return Json(devices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Subscribe([FromBody] Subscription subscription)
        {
            try
            {
                var device = db.Devices.FirstOrDefault(x => x.Endpoint == subscription.Endpoint);
                if (device == null)
                {
                    device = subscription.GetDevice();
                    db.Devices.Add(device);
                }
                device.LastSync = DateTimeOffset.Now;
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Unsubscribe([FromBody] Subscription subscription)
        {
            try
            {
                var device = db.Devices.FirstOrDefault(x => x.Endpoint == subscription.Endpoint);
                db.Devices.Remove(device);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        public class PushPayload {
            public Guid ID { get; set; }
            public bool withPause { get; set; }
        }

    }
}