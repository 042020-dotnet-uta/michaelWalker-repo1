using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendorApp.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "AspNetRoles",
          columns: table => new
          {
            Id = table.Column<string>(nullable: false),
            Name = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
            ConcurrencyStamp = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetRoles", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUsers",
          columns: table => new
          {
            Id = table.Column<string>(nullable: false),
            UserName = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
            Email = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
            EmailConfirmed = table.Column<bool>(nullable: false),
            PasswordHash = table.Column<string>(nullable: true),
            SecurityStamp = table.Column<string>(nullable: true),
            ConcurrencyStamp = table.Column<string>(nullable: true),
            PhoneNumber = table.Column<string>(nullable: true),
            PhoneNumberConfirmed = table.Column<bool>(nullable: false),
            TwoFactorEnabled = table.Column<bool>(nullable: false),
            LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
            LockoutEnabled = table.Column<bool>(nullable: false),
            AccessFailedCount = table.Column<int>(nullable: false),
            NumCartItems = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetUsers", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Catagories",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            CatagoryName = table.Column<string>(nullable: false),
            HexColorTheme = table.Column<string>(maxLength: 7, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Catagories", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "Locations",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Locations", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "Products",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(maxLength: 50, nullable: false),
            Catagory = table.Column<string>(maxLength: 50, nullable: false),
            FAClass = table.Column<string>(maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Products", x => x.ID);
          });

      migrationBuilder.CreateTable(
          name: "AspNetRoleClaims",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            RoleId = table.Column<string>(nullable: false),
            ClaimType = table.Column<string>(nullable: true),
            ClaimValue = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "AspNetRoles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserClaims",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            UserId = table.Column<string>(nullable: false),
            ClaimType = table.Column<string>(nullable: true),
            ClaimValue = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserLogins",
          columns: table => new
          {
            LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
            ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
            ProviderDisplayName = table.Column<string>(nullable: true),
            UserId = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            table.ForeignKey(
                      name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserRoles",
          columns: table => new
          {
            UserId = table.Column<string>(nullable: false),
            RoleId = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            table.ForeignKey(
                      name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "AspNetRoles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserTokens",
          columns: table => new
          {
            UserId = table.Column<string>(nullable: false),
            LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
            Name = table.Column<string>(maxLength: 128, nullable: false),
            Value = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            table.ForeignKey(
                      name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Carts",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            UserId = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Carts", x => x.ID);
            table.ForeignKey(
                      name: "FK_Carts_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Orders",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            UserId = table.Column<string>(nullable: false),
            CreatedDate = table.Column<DateTime>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Orders", x => x.ID);
            table.ForeignKey(
                      name: "FK_Orders_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "LocationInventoryRecords",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            LocationID = table.Column<int>(nullable: false),
            ProductID = table.Column<int>(nullable: false),
            Quanitity = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_LocationInventoryRecords", x => x.ID);
            table.ForeignKey(
                      name: "FK_LocationInventoryRecords_Locations_LocationID",
                      column: x => x.LocationID,
                      principalTable: "Locations",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_LocationInventoryRecords_Products_ProductID",
                      column: x => x.ProductID,
                      principalTable: "Products",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "ProductCatagories",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            ProductID = table.Column<int>(nullable: false),
            CatagoryID = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ProductCatagories", x => x.ID);
            table.ForeignKey(
                      name: "FK_ProductCatagories_Catagories_CatagoryID",
                      column: x => x.CatagoryID,
                      principalTable: "Catagories",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ProductCatagories_Products_ProductID",
                      column: x => x.ProductID,
                      principalTable: "Products",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "CartItems",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            CartID = table.Column<int>(nullable: false),
            LocationName = table.Column<string>(maxLength: 50, nullable: false),
            ProductName = table.Column<string>(maxLength: 50, nullable: false),
            AmountPurchased = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_CartItems", x => x.ID);
            table.ForeignKey(
                      name: "FK_CartItems_Carts_CartID",
                      column: x => x.CartID,
                      principalTable: "Carts",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "OrderItem",
          columns: table => new
          {
            ID = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            ProductName = table.Column<string>(nullable: false),
            LocationName = table.Column<string>(nullable: false),
            AmountPurchased = table.Column<int>(nullable: false),
            OrderID = table.Column<int>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OrderItem", x => x.ID);
            table.ForeignKey(
                      name: "FK_OrderItem_Orders_OrderID",
                      column: x => x.OrderID,
                      principalTable: "Orders",
                      principalColumn: "ID",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_AspNetRoleClaims_RoleId",
          table: "AspNetRoleClaims",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "RoleNameIndex",
          table: "AspNetRoles",
          column: "NormalizedName",
          unique: true,
          filter: "[NormalizedName] IS NOT NULL");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserClaims_UserId",
          table: "AspNetUserClaims",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserLogins_UserId",
          table: "AspNetUserLogins",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserRoles_RoleId",
          table: "AspNetUserRoles",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "EmailIndex",
          table: "AspNetUsers",
          column: "NormalizedEmail");

      migrationBuilder.CreateIndex(
          name: "UserNameIndex",
          table: "AspNetUsers",
          column: "NormalizedUserName",
          unique: true,
          filter: "[NormalizedUserName] IS NOT NULL");

      migrationBuilder.CreateIndex(
          name: "IX_CartItems_CartID",
          table: "CartItems",
          column: "CartID");

      migrationBuilder.CreateIndex(
          name: "IX_Carts_UserId",
          table: "Carts",
          column: "UserId",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_LocationInventoryRecords_LocationID",
          table: "LocationInventoryRecords",
          column: "LocationID");

      migrationBuilder.CreateIndex(
          name: "IX_LocationInventoryRecords_ProductID",
          table: "LocationInventoryRecords",
          column: "ProductID");

      migrationBuilder.CreateIndex(
          name: "IX_OrderItem_OrderID",
          table: "OrderItem",
          column: "OrderID");

      migrationBuilder.CreateIndex(
          name: "IX_Orders_UserId",
          table: "Orders",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_ProductCatagories_CatagoryID",
          table: "ProductCatagories",
          column: "CatagoryID");

      migrationBuilder.CreateIndex(
          name: "IX_ProductCatagories_ProductID",
          table: "ProductCatagories",
          column: "ProductID");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "AspNetRoleClaims");

      migrationBuilder.DropTable(
          name: "AspNetUserClaims");

      migrationBuilder.DropTable(
          name: "AspNetUserLogins");

      migrationBuilder.DropTable(
          name: "AspNetUserRoles");

      migrationBuilder.DropTable(
          name: "AspNetUserTokens");

      migrationBuilder.DropTable(
          name: "CartItems");

      migrationBuilder.DropTable(
          name: "LocationInventoryRecords");

      migrationBuilder.DropTable(
          name: "OrderItem");

      migrationBuilder.DropTable(
          name: "ProductCatagories");

      migrationBuilder.DropTable(
          name: "AspNetRoles");

      migrationBuilder.DropTable(
          name: "Carts");

      migrationBuilder.DropTable(
          name: "Locations");

      migrationBuilder.DropTable(
          name: "Orders");

      migrationBuilder.DropTable(
          name: "Catagories");

      migrationBuilder.DropTable(
          name: "Products");

      migrationBuilder.DropTable(
          name: "AspNetUsers");
    }
  }
}
