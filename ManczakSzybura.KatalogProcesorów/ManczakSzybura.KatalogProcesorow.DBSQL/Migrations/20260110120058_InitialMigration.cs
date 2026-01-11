using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManczakSzybura.KatalogProcesorow.DBSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cPUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    manufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cores = table.Column<int>(type: "INTEGER", nullable: false),
                    Threads = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseClockGHz = table.Column<double>(type: "REAL", nullable: false),
                    SocketType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cPUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cPUs");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
