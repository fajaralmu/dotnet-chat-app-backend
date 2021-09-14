using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAPI.Migrations
{
    public partial class fixing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "chat_message",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_user_id",
                table: "chat_message",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_chat_message_users_user_id",
                table: "chat_message",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_message_users_user_id",
                table: "chat_message");

            migrationBuilder.DropIndex(
                name: "IX_chat_message_user_id",
                table: "chat_message");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "chat_message");
        }
    }
}
