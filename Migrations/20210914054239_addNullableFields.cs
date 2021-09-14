using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAPI.Migrations
{
    public partial class addNullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_message_chat_room_chat_room_id",
                table: "chat_message");

            migrationBuilder.DropForeignKey(
                name: "FK_chat_message_users_to_user_id",
                table: "chat_message");

            migrationBuilder.AlterColumn<int>(
                name: "to_user_id",
                table: "chat_message",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "chat_room_id",
                table: "chat_message",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_chat_message_chat_room_chat_room_id",
                table: "chat_message",
                column: "chat_room_id",
                principalTable: "chat_room",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_chat_message_users_to_user_id",
                table: "chat_message",
                column: "to_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_message_chat_room_chat_room_id",
                table: "chat_message");

            migrationBuilder.DropForeignKey(
                name: "FK_chat_message_users_to_user_id",
                table: "chat_message");

            migrationBuilder.AlterColumn<int>(
                name: "to_user_id",
                table: "chat_message",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "chat_room_id",
                table: "chat_message",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_chat_message_chat_room_chat_room_id",
                table: "chat_message",
                column: "chat_room_id",
                principalTable: "chat_room",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chat_message_users_to_user_id",
                table: "chat_message",
                column: "to_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
