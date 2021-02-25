using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoApi.Enums;
using ToDoCore.Models;

namespace ToDoApi.Services
{
    public interface IToDoListService
    {
        IEnumerable<ToDoListDto> GetAllLists(string ownerEmail);
        ToDoListDto GetListById(Guid id, string ownerEmail);
        ToDoListDto CreateList(ToDoListDto toDoListDto, string ownerEmail);    
        ToDoListDto UpdateList(Guid id, ToDoListDto toDoListDto, string ownerEmail);
        bool DeleteList(Guid id, string ownerEmail);
        IEnumerable<ToDoListDto> SearchLists(string title, string ownerEmail);
        EResponse UpdateListPosition(Guid listId, int newPosition, string ownerEmail);
        ToDoListDto GetSharedToDoList(Guid shareToDoListId);
        ShareToDoList ShareToDoList(Guid id, string ownerEmail);
    }
}
