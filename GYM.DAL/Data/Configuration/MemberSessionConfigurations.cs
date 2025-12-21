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
    public class MemberSessionConfigurations : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(ms => ms.CreateAt).HasColumnName("BookingDate")
                                                .HasDefaultValueSql("GETDATE()");

            builder.HasKey(ms => new { ms.MemberId, ms.SessionId });
            builder.Ignore(ms => ms.Id);
        }
    }
}
