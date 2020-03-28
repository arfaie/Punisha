using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Brands",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Cars",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "CategoryGroups",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "FieldGroups",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "FieldTypes",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Offers",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "SelectGroups",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Sliders",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "States",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Statuses",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Units",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "UserGroups",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Categories",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    CategoryGroupId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        "FK_Categories_CategoryGroups_CategoryGroupId",
                        x => x.CategoryGroupId,
                        "CategoryGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Fields",
                table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SelectGroupId = table.Column<string>(nullable: true),
                    FieldGroupId = table.Column<string>(nullable: true),
                    FieldTypeId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                    table.ForeignKey(
                        "FK_Fields_FieldGroups_FieldGroupId",
                        x => x.FieldGroupId,
                        "FieldGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Fields_FieldTypes_FieldTypeId",
                        x => x.FieldTypeId,
                        "FieldTypes",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Fields_SelectGroups_SelectGroupId",
                        x => x.SelectGroupId,
                        "SelectGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SelectItems",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    SelectGroupId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectItems", x => x.Id);
                    table.ForeignKey(
                        "FK_SelectItems_SelectGroups_SelectGroupId",
                        x => x.SelectGroupId,
                        "SelectGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Cities",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: false),
                    StateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        "FK_Cities_States_StateId",
                        x => x.StateId,
                        "States",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
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
                    FullName = table.Column<string>(nullable: true),
                    NationalCode = table.Column<long>(nullable: false),
                    TaxiCode = table.Column<string>(nullable: true),
                    CarId = table.Column<string>(nullable: true),
                    UserGroupId = table.Column<string>(nullable: true),
                    RegistrationDateAndTime = table.Column<DateTime>(nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUsers_Cars_CarId",
                        x => x.CarId,
                        "Cars",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AspNetUsers_UserGroups_UserGroupId",
                        x => x.UserGroupId,
                        "UserGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Products",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    CategoryId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    UnitId = table.Column<string>(nullable: true),
                    Inventory = table.Column<int>(nullable: false),
                    IsSellable = table.Column<bool>(nullable: false),
                    OrderPoint = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    OldPrice = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        "FK_Products_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Products_Units_UnitId",
                        x => x.UnitId,
                        "Units",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "CategoryFields",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    CategoryId = table.Column<string>(nullable: true),
                    FieldId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFields", x => x.Id);
                    table.ForeignKey(
                        "FK_CategoryFields_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CategoryFields_Fields_FieldId",
                        x => x.FieldId,
                        "Fields",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Agencies",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    PostalCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                    table.ForeignKey(
                        "FK_Agencies_Cities_CityId",
                        x => x.CityId,
                        "Cities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Addresses",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Recipient = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        "FK_Addresses_Cities_CityId",
                        x => x.CityId,
                        "Cities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoles_AspNetUsers_ApplicationUserId",
                        x => x.ApplicationUserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
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
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Factors",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<int>(nullable: false),
                    TotalDiscount = table.Column<int>(nullable: false),
                    Tax = table.Column<float>(nullable: false),
                    FinalPrice = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FactorCode = table.Column<string>(nullable: true),
                    IsPaid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        "FK_Factors_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "InventoryChanges",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    ProductId = table.Column<string>(nullable: true),
                    Old = table.Column<int>(nullable: false),
                    New = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryChanges", x => x.Id);
                    table.ForeignKey(
                        "FK_InventoryChanges_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "OfferItems",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    DiscountAmount = table.Column<decimal>(nullable: false),
                    DiscountPercent = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferItems", x => x.Id);
                    table.ForeignKey(
                        "FK_OfferItems_Offers_OfferId",
                        x => x.OfferId,
                        "Offers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_OfferItems_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "PriceChanges",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    ProductId = table.Column<string>(nullable: true),
                    Old = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChanges", x => x.Id);
                    table.ForeignKey(
                        "FK_PriceChanges_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ProductFields",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    ProductId = table.Column<string>(nullable: true),
                    FieldId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CarNames = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFields", x => x.Id);
                    table.ForeignKey(
                        "FK_ProductFields_Fields_FieldId",
                        x => x.FieldId,
                        "Fields",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_ProductFields_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ProductGalleries",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    ProductId = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGalleries", x => x.Id);
                    table.ForeignKey(
                        "FK_ProductGalleries_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
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
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "FactorItems",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    FactorId = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    UnitCount = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<int>(nullable: false),
                    Discount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorItems", x => x.Id);
                    table.ForeignKey(
                        "FK_FactorItems_Factors_FactorId",
                        x => x.FactorId,
                        "Factors",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_FactorItems_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Orders",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    FactorId = table.Column<string>(nullable: true),
                    TransactionNumber = table.Column<string>(nullable: true),
                    TransactionStatus = table.Column<bool>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<string>(nullable: true),
                    IssueCode = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        "FK_Orders_Factors_FactorId",
                        x => x.FactorId,
                        "Factors",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Orders_Statuses_StatusId",
                        x => x.StatusId,
                        "Statuses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ProductSelectedItems",
                table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    ProductFieldId = table.Column<string>(nullable: true),
                    SelectItemId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSelectedItems", x => x.Id);
                    table.ForeignKey(
                        "FK_ProductSelectedItems_ProductFields_ProductFieldId",
                        x => x.ProductFieldId,
                        "ProductFields",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_ProductSelectedItems_SelectItems_SelectItemId",
                        x => x.SelectItemId,
                        "SelectItems",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Addresses_CityId",
                "Addresses",
                "CityId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_UserId",
                "Addresses",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_Agencies_CityId",
                "Agencies",
                "CityId");

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_AspNetRoles_ApplicationUserId",
                "AspNetRoles",
                "ApplicationUserId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUsers_CarId",
                "AspNetUsers",
                "CarId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AspNetUsers_UserGroupId",
                "AspNetUsers",
                "UserGroupId");

            migrationBuilder.CreateIndex(
                "IX_Categories_CategoryGroupId",
                "Categories",
                "CategoryGroupId");

            migrationBuilder.CreateIndex(
                "IX_CategoryFields_CategoryId",
                "CategoryFields",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_CategoryFields_FieldId",
                "CategoryFields",
                "FieldId");

            migrationBuilder.CreateIndex(
                "IX_Cities_StateId",
                "Cities",
                "StateId");

            migrationBuilder.CreateIndex(
                "IX_FactorItems_FactorId",
                "FactorItems",
                "FactorId");

            migrationBuilder.CreateIndex(
                "IX_FactorItems_ProductId",
                "FactorItems",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_Factors_UserId",
                "Factors",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_Fields_FieldGroupId",
                "Fields",
                "FieldGroupId");

            migrationBuilder.CreateIndex(
                "IX_Fields_FieldTypeId",
                "Fields",
                "FieldTypeId");

            migrationBuilder.CreateIndex(
                "IX_Fields_SelectGroupId",
                "Fields",
                "SelectGroupId");

            migrationBuilder.CreateIndex(
                "IX_InventoryChanges_ProductId",
                "InventoryChanges",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_OfferItems_OfferId",
                "OfferItems",
                "OfferId");

            migrationBuilder.CreateIndex(
                "IX_OfferItems_ProductId",
                "OfferItems",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_Orders_FactorId",
                "Orders",
                "FactorId");

            migrationBuilder.CreateIndex(
                "IX_Orders_StatusId",
                "Orders",
                "StatusId");

            migrationBuilder.CreateIndex(
                "IX_PriceChanges_ProductId",
                "PriceChanges",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_ProductFields_FieldId",
                "ProductFields",
                "FieldId");

            migrationBuilder.CreateIndex(
                "IX_ProductFields_ProductId",
                "ProductFields",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_ProductGalleries_ProductId",
                "ProductGalleries",
                "ProductId");

            migrationBuilder.CreateIndex(
                "IX_Products_CategoryId",
                "Products",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_Products_UnitId",
                "Products",
                "UnitId");

            migrationBuilder.CreateIndex(
                "IX_ProductSelectedItems_ProductFieldId",
                "ProductSelectedItems",
                "ProductFieldId");

            migrationBuilder.CreateIndex(
                "IX_ProductSelectedItems_SelectItemId",
                "ProductSelectedItems",
                "SelectItemId");

            migrationBuilder.CreateIndex(
                "IX_SelectItems_SelectGroupId",
                "SelectItems",
                "SelectGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Addresses");

            migrationBuilder.DropTable(
                "Agencies");

            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "Brands");

            migrationBuilder.DropTable(
                "CategoryFields");

            migrationBuilder.DropTable(
                "FactorItems");

            migrationBuilder.DropTable(
                "InventoryChanges");

            migrationBuilder.DropTable(
                "OfferItems");

            migrationBuilder.DropTable(
                "Orders");

            migrationBuilder.DropTable(
                "PriceChanges");

            migrationBuilder.DropTable(
                "ProductGalleries");

            migrationBuilder.DropTable(
                "ProductSelectedItems");

            migrationBuilder.DropTable(
                "Sliders");

            migrationBuilder.DropTable(
                "Cities");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "Offers");

            migrationBuilder.DropTable(
                "Factors");

            migrationBuilder.DropTable(
                "Statuses");

            migrationBuilder.DropTable(
                "ProductFields");

            migrationBuilder.DropTable(
                "SelectItems");

            migrationBuilder.DropTable(
                "States");

            migrationBuilder.DropTable(
                "AspNetUsers");

            migrationBuilder.DropTable(
                "Fields");

            migrationBuilder.DropTable(
                "Products");

            migrationBuilder.DropTable(
                "Cars");

            migrationBuilder.DropTable(
                "UserGroups");

            migrationBuilder.DropTable(
                "FieldGroups");

            migrationBuilder.DropTable(
                "FieldTypes");

            migrationBuilder.DropTable(
                "SelectGroups");

            migrationBuilder.DropTable(
                "Categories");

            migrationBuilder.DropTable(
                "Units");

            migrationBuilder.DropTable(
                "CategoryGroups");
        }
    }
}
