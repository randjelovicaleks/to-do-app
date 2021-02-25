using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCore.Models
{
    public class ToDoList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IList<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
        public int Position { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public bool IsReminded { get; set; }
        public string Owner { get; set; }
        public IList<ShareToDoList> ShareToDoLists {get; set;}
  
        public ToDoList() { }

        public ToDoList Update(ToDoList toDoList)
        {
            Title = toDoList.Title;
            ReminderDateTime = toDoList.ReminderDateTime;
            IsReminded = toDoList.IsReminded;
            return this;
        }  
    }
}
