﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pdam.Customer.Service.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Router_RouterId",
                table: "Customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouterId",
                table: "Customers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "AssetTypeCode",
                table: "CustomerAssets",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Router_RouterId",
                table: "Customers",
                column: "RouterId",
                principalTable: "Router",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Router_RouterId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AssetTypeCode",
                table: "CustomerAssets");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouterId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Router_RouterId",
                table: "Customers",
                column: "RouterId",
                principalTable: "Router",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
