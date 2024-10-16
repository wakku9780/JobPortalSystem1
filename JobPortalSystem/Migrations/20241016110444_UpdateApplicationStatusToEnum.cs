using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing string statuses to equivalent int values
            migrationBuilder.Sql("UPDATE Applications SET Status = 0 WHERE Status = 'Applied'");
            migrationBuilder.Sql("UPDATE Applications SET Status = 1 WHERE Status = 'Interview'");
            migrationBuilder.Sql("UPDATE Applications SET Status = 2 WHERE Status = 'Rejected'");
            migrationBuilder.Sql("UPDATE Applications SET Status = 3 WHERE Status = 'Accepted'");

            // Now change the column type
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Applications",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Convert int values back to string in case of rollback
            migrationBuilder.Sql("UPDATE Applications SET Status = 'Applied' WHERE Status = 0");
            migrationBuilder.Sql("UPDATE Applications SET Status = 'Interview' WHERE Status = 1");
            migrationBuilder.Sql("UPDATE Applications SET Status = 'Rejected' WHERE Status = 2");
            migrationBuilder.Sql("UPDATE Applications SET Status = 'Accepted' WHERE Status = 3");

            // Revert the column type back to string
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Applications",
                nullable: true,
                oldClrType: typeof(int));
        }

    }
}
