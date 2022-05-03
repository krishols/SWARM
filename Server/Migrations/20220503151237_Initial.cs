using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWARM.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "C##_UD_KRIS");

            migrationBuilder.CreateTable(
                name: "ASP_NET_ROLES",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_USERS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    USER_NAME = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DEVICE_CODES",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    USER_CODE = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DEVICE_CODE = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SUBJECT_ID = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    SESSION_ID = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CLIENT_ID = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CREATION_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EXPIRATION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA = table.Column<string>(type: "NCLOB", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEVICE_CODES", x => x.USER_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PERSISTED_GRANTS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    KEY = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    TYPE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    SUBJECT_ID = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    SESSION_ID = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CLIENT_ID = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CREATION_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EXPIRATION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CONSUMED_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATA = table.Column<string>(type: "NCLOB", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSISTED_GRANTS", x => x.KEY);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_ROLE_CLAIMS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ROLE_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_ROLE_CLAIMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ASP_NET_ROLE_CLAIMS_ASP_NET_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_USER_CLAIMS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_USER_CLAIMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ASP_NET_USER_CLAIMS_ASP_NET_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_USER_LOGINS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_USER_LOGINS", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_ASP_NET_USER_LOGINS_ASP_NET_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_USER_ROLES",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ROLE_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_USER_ROLES", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_ASP_NET_USER_ROLES_ASP_NET_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ASP_NET_USER_ROLES_ASP_NET_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASP_NET_USER_TOKENS",
                schema: "C##_UD_KRIS",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASP_NET_USER_TOKENS", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_ASP_NET_USER_TOKENS_ASP_NET_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "C##_UD_KRIS",
                        principalTable: "ASP_NET_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASP_NET_ROLE_CLAIMS_ROLE_ID",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_ROLE_CLAIMS",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "ROLENAMEINDEX",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_ROLES",
                column: "NORMALIZED_NAME",
                unique: true,
                filter: "\"NORMALIZED_NAME\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ASP_NET_USER_CLAIMS_USER_ID",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_USER_CLAIMS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ASP_NET_USER_LOGINS_USER_ID",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_USER_LOGINS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ASP_NET_USER_ROLES_ROLE_ID",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_USER_ROLES",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "EMAILINDEX",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_USERS",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "USERNAMEINDEX",
                schema: "C##_UD_KRIS",
                table: "ASP_NET_USERS",
                column: "NORMALIZED_USER_NAME",
                unique: true,
                filter: "\"NORMALIZED_USER_NAME\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DEVICE_CODES_DEVICE_CODE",
                schema: "C##_UD_KRIS",
                table: "DEVICE_CODES",
                column: "DEVICE_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DEVICE_CODES_EXPIRATION",
                schema: "C##_UD_KRIS",
                table: "DEVICE_CODES",
                column: "EXPIRATION");

            migrationBuilder.CreateIndex(
                name: "IX_PERSISTED_GRANTS_EXPIRATION",
                schema: "C##_UD_KRIS",
                table: "PERSISTED_GRANTS",
                column: "EXPIRATION");

            migrationBuilder.CreateIndex(
                name: "IX_PERSISTED_GRANTS_SUBJECT_ID_CLIENT_ID_TYPE",
                schema: "C##_UD_KRIS",
                table: "PERSISTED_GRANTS",
                columns: new[] { "SUBJECT_ID", "CLIENT_ID", "TYPE" });

            migrationBuilder.CreateIndex(
                name: "IX_PERSISTED_GRANTS_SUBJECT_ID_SESSION_ID_TYPE",
                schema: "C##_UD_KRIS",
                table: "PERSISTED_GRANTS",
                columns: new[] { "SUBJECT_ID", "SESSION_ID", "TYPE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASP_NET_ROLE_CLAIMS",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_USER_CLAIMS",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_USER_LOGINS",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_USER_ROLES",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_USER_TOKENS",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "DEVICE_CODES",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "PERSISTED_GRANTS",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_ROLES",
                schema: "C##_UD_KRIS");

            migrationBuilder.DropTable(
                name: "ASP_NET_USERS",
                schema: "C##_UD_KRIS");
        }
    }
}
