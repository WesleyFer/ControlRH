using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlRH.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tipo_Permissao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Permissao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Permissao");
        }
    }
}
