using Microsoft.EntityFrameworkCore.Migrations;

namespace Clientes.DAL.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha_de_Creación",
                table: "Clientes",
                newName: "Fecha_de_Creacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha_de_Creacion",
                table: "Clientes",
                newName: "Fecha_de_Creación");
        }
    }
}
