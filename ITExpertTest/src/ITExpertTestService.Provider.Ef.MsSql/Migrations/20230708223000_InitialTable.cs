using ITExpertTestService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITExpertTestService.Provider.Ef.MsSql.Migrations
{
    [DbContext(typeof(ITExpertServiceDbContext))]
    [Migration("20230708223000_InitialTable")]
    public class InitialTable : Migration
    {
        protected override void Up(MigrationBuilder builder)
        {
            builder.CreateTable(
                name: DbObject.TableName,
                columns: table => new
                {
                    SerialNumber = table.Column<int>(nullable: false)
                        .Annotation(
                            "SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{DbObject.TableName}", x => x.SerialNumber);
                });
        }

        protected override void Down(MigrationBuilder builder)
        {
            builder.DropTable(
                name: DbObject.TableName);
        }
    }
}
