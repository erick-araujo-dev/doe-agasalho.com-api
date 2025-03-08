using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeAgasalhoApiV2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioResponsavelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuario_responsavel",
                table: "ponto_coleta",
                type: "varchar(50)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
