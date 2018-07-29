using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PushNotifications.Migrations
{
    public partial class AddTableDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Endpoint = table.Column<string>(nullable: true),
                    P256dh = table.Column<string>(nullable: true),
                    Auth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
