using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeAgasalhoApiV2._0.Migrations
{
    /// <inheritdoc />
    public partial class add_cadastro_enum_typeMovimento_tableDoacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<string>(
                name: "tipo_movimento",
                table: "doacao",
                type: "enum('entrada','saida', 'cadastro')",
                nullable: false,
                collation: "utf8mb3_general_ci",
                oldClrType: typeof(string),
                oldType: "enum('entrada','saida')")
                .Annotation("MySql:CharSet", "utf8mb3")
                .OldAnnotation("MySql:CharSet", "utf8mb3")
                .OldAnnotation("Relational:Collation", "utf8mb3_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo_movimento",
                table: "doacao",
                type: "enum('entrada','saida')",
                nullable: false,
                collation: "utf8mb3_general_ci",
                oldClrType: typeof(string),
                oldType: "enum('entrada','saida', 'cadastro')")
                .Annotation("MySql:CharSet", "utf8mb3")
                .OldAnnotation("MySql:CharSet", "utf8mb3")
                .OldAnnotation("Relational:Collation", "utf8mb3_general_ci");
        }
    }
}
