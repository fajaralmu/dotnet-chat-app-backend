using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAPI.Migrations
{
    public partial class ADDUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_room_code",
                table: "chat_room",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_profile_code",
                table: "application_profile",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_chat_room_code",
                table: "chat_room");

            migrationBuilder.DropIndex(
                name: "IX_application_profile_code",
                table: "application_profile");
        }
    }
}
