using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoCore.Models;

namespace ToDoInfrastructure.Configurations
{
    class ShareToDoListConfiguration : IEntityTypeConfiguration<ShareToDoList>
    {
        public void Configure(EntityTypeBuilder<ShareToDoList> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(s => s.ToDoList)
                .WithMany(l => l.ShareToDoLists)
                .HasForeignKey(s => s.ToDoListId);
        }
    }
}
