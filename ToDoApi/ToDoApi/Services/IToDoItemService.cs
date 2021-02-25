using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoApi.Enums;

namespace ToDoApi.Services
{
    public interface IToDoItemService
    {
        IEnumerable<ToDoItemDto> GetAllItems(Guid listId, string ownerEmail);
        ToDoItemDto GetItemById(Guid listId, Guid itemId, string ownerEmail);
        ToDoItemDto CreateItem(Guid listId, ToDoItemDto toDoItemDto, string ownerEmail);
        ToDoItemDto UpdateItem(Guid listId, Guid itemId, ToDoItemDto toDoItemDto, string ownerEmail);
        bool DeleteItem(Guid listId, Guid itemId, string ownerEmail);
        EResponse UpdateItemPosition(Guid listId, Guid itemId, int newPosition, string ownerEmail);
    }
}
