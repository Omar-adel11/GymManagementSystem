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
    public class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                tb.HasCheckConstraint("SessionDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId);

            builder.HasOne(s => s.Trainer)
                   .WithMany(t => t.Sessions)
                   .HasForeignKey(s => s.TrainerId);
        }
    }
}
