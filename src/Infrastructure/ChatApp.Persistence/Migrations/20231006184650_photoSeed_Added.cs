using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChatApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class photoSeed_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "MessageSend",
                value: new DateTime(2023, 10, 6, 18, 46, 49, 725, DateTimeKind.Utc).AddTicks(3118));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2,
                column: "MessageSend",
                value: new DateTime(2023, 10, 6, 18, 46, 49, 725, DateTimeKind.Utc).AddTicks(3127));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 3,
                column: "MessageSend",
                value: new DateTime(2023, 10, 6, 18, 46, 49, 725, DateTimeKind.Utc).AddTicks(3133));

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "AppUserId", "IsActive", "IsMain", "ModifiedDate", "PublicId", "Url" },
                values: new object[,]
                {
                    { 1, "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "https://xsgames.co/randomusers/assets/avatars/male/1.jpg" },
                    { 2, "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "https://xsgames.co/randomusers/assets/avatars/male/50.jpg" },
                    { 3, "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "https://xsgames.co/randomusers/assets/avatars/male/60.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "MessageSend",
                value: new DateTime(2023, 10, 2, 14, 23, 16, 222, DateTimeKind.Utc).AddTicks(3013));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2,
                column: "MessageSend",
                value: new DateTime(2023, 10, 2, 14, 23, 16, 222, DateTimeKind.Utc).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 3,
                column: "MessageSend",
                value: new DateTime(2023, 10, 2, 14, 23, 16, 222, DateTimeKind.Utc).AddTicks(3056));
        }
    }
}
