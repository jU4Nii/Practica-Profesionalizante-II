using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberManagerAPIs.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Egresos",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "FechaCierre",
                table: "Cajas");

            migrationBuilder.RenameColumn(
                name: "Ingresos",
                table: "Cajas",
                newName: "Monto");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "Cajas",
                newName: "Fecha");

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "Cajas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EsIngreso",
                table: "Cajas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Cajas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "EsIngreso",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Cajas");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "Cajas",
                newName: "Ingresos");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Cajas",
                newName: "FechaInicio");

            migrationBuilder.AddColumn<decimal>(
                name: "Egresos",
                table: "Cajas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierre",
                table: "Cajas",
                type: "datetime2",
                nullable: true);
        }
    }
}
