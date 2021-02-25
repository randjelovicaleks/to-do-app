using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoCore.Models;

namespace Test.Configurations
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
      
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Title)
                .HasMaxLength(20);
            builder.Property(l => l.Owner)
                .IsRequired();
        }
    }
}
