using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberManagerAPIs.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCanceladoTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelado",
                table: "Turnos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelado",
                table: "Turnos");
        }
    }
}
