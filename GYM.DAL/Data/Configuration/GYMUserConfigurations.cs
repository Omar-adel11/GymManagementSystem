using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GYM.DAL.Data.Configuration
{
    public class GYMUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GYMUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.Name).IsRequired()
                                         .HasColumnType("varchar")
                                         .HasMaxLength(50);

            builder.Property(p => p.Email).IsRequired()
                                        .HasColumnType("varchar")
                                        .HasMaxLength(100);

            builder.Property(p => p.phone).IsRequired()
                                        .HasColumnType("varchar")
                                        .HasMaxLength(11);
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GYMUserValidEmailCheck", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("GYMUserValidPhoneCheck", "phone Like '01%' and phone NOT LIKE '%[^0-9]%' ");
            });

            builder.OwnsOne(p => p.Address, A =>
            {
                A.Property(a => a.Street).HasColumnType("varchar").HasMaxLength(30);
                A.Property(a => a.City).HasColumnType("varchar").HasMaxLength(30);
            });

            builder.HasIndex(p => p.Email).IsUnique();
            builder.HasIndex(p => p.phone).IsUnique();

        }
    }
}
