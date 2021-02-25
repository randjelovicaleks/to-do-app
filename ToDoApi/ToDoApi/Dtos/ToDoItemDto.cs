using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Dtos
{
    public class ToDoItemDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public int Position { get; set; }
    }
}
