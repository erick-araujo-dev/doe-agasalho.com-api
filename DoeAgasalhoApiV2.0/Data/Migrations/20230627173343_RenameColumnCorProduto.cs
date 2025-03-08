using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeAgasalhoApiV2._0.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnCorProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cor",
                table: "produto",
                newName: "caracteristica");

            migrationBuilder.AlterColumn<string>(
                name: "caracteristica",
                table: "produto",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "caracteristica",
                table: "produto",
                newName: "cor");

            migrationBuilder.AlterColumn<string>(
                name: "cor",
                table: "produto",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);
        }
    }
}
