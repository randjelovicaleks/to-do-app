using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoCore.Models;

namespace Test.Configurations
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Text)
                .IsRequired();
            builder.HasOne(i => i.ToDoList)
                .WithMany(l => l.ToDoItems)
                .HasForeignKey(i => i.ToDoListId);

        }
    }
}
