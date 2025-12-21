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
    public class MemberConfigurations : GYMUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder) 
        {
            builder.Property(m => m.CreateAt).HasColumnName("JoinDate")
                                             .HasDefaultValueSql("GETDATE()");
            base.Configure(builder);

            
        }
    }
}
