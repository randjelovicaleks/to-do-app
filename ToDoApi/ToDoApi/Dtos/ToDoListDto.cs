using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Dtos
{
    public class ToDoListDto
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }
        public IList<ToDoItemDto> ToDoItems { get; set; }
        public int Position { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public bool isReminded { get; set; }
      
    }
}
