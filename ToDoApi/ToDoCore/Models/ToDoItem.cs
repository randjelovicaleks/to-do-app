using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCore.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
        public int Position { get; set; } 

        public ToDoItem() { }

        public ToDoItem Update(ToDoItem toDoItem)
        {
            Text = toDoItem.Text;
            IsDone = toDoItem.IsDone;
            return this;
        }

    }
}
