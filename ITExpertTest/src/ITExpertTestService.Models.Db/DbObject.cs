using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITExpertTestService.Models.Db
{
    public class DbObject
    {
        public const string TableName = "Object";

        public int SerialNumber { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }

    public class DbObjectConfiguration : IEntityTypeConfiguration<DbObject>
    {
        public void Configure(EntityTypeBuilder<DbObject> builder)
        {
            builder
              .ToTable(DbObject.TableName);

            builder
              .HasKey(t => t.SerialNumber);

            builder
                .Property(t => t.SerialNumber)
                .ValueGeneratedOnAdd();
        }
    }
}
