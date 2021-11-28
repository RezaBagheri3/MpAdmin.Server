﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MpAdmin.Server.DAL.Migrations
{
    public partial class AddCustomerFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    Final = table.Column<int>(type: "int", nullable: false),
                    TotalProfit = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    PayableAmount = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactorWallPapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WallPaperCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<int>(type: "int", nullable: false),
                    Profit = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    FactorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorWallPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactorWallPapers_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CustomerId",
                table: "Factors",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorWallPapers_FactorId",
                table: "FactorWallPapers",
                column: "FactorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactorWallPapers");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
