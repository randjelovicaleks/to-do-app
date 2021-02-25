using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCore.Models
{
    public class ShareToDoList
    {
        public Guid Id { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
        public DateTime ExpiryDateTime { get; set; }

        public ShareToDoList() { }
    }
}
