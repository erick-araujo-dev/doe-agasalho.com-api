using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeAgasalhoApiV2._0.Migrations
{
    /// <inheritdoc />
    public partial class alter_type_dataMovimento_Doacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_movimento",
                table: "doacao",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "data_movimento",
                table: "doacao",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
