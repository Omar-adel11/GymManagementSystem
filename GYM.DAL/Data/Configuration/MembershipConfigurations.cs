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
    public class MembershipConfigurations : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(ms => ms.CreateAt).HasColumnName("StartDate")
                                               .HasDefaultValueSql("GETDATE()") ;

            builder.HasKey(x => new { x.MemberId, x.PlanId });
            builder.Ignore(x => x.Id);
        }
    }
}
