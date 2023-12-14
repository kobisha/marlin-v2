using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Marlin.sqlite.Migrations
{
    /// <inheritdoc />
    public partial class exchangelog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionID",
                table: "ExchangeLog",
                newName: "JsonBody");

            migrationBuilder.RenameColumn(
                name: "MessageID",
                table: "ExchangeLog",
                newName: "AccountID");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

           

            migrationBuilder.RenameColumn(
                name: "JsonBody",
                table: "ExchangeLog",
                newName: "TransactionID");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "ExchangeLog",
                newName: "MessageID");

        }
    }
}
