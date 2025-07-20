using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.ValueObjects;

namespace PingPong_Authentication_Infrastructure.Persistence.DataBaseSettings
{
    internal class UsersSettings : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasIndex(s => s.Id).HasDatabaseName("idx_users_id");

            builder.ToTable("users");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()").IsRequired();

            builder.Property(s => s.Email).HasColumnName("email").HasMaxLength(50).HasConversion(email => email.Value, value => Email.Create(value)!).IsRequired();

            builder.Property(s => s.Nickname).HasColumnName("nickname").HasMaxLength(50).IsRequired();

            builder.Property(s => s.Hash).HasColumnName("hash").HasMaxLength(80).IsRequired();

            builder.Property(s => s.Salt).HasColumnName("salt").HasMaxLength(80).IsRequired();

            builder.Property(s => s.State).HasColumnName("state");
        }
    }
}
