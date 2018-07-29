using System;
using System.Linq;
using DataLevelLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Push_Notifications.DataLevelLogic
{
    public class PushContext : DbContext
    {
        internal readonly DbContextOptions<PushContext> Options;

        public PushContext(DbContextOptions<PushContext> options) : base(options)
        {
            Options = options;
        }

        public DbSet<Device> Devices { get; set; }
    }
}
