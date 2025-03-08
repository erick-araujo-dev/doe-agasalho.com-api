using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeAgasalhoApiV2._0.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsuarioResponsavelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "usuario_responsavel",
            table: "ponto_coleta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
