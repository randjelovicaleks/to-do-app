using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoApi.Enums;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly ToDoDbContext _toDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ToDoItemService> _logger;

        public ToDoItemService(ToDoDbContext toDoDbContext, IMapper mapper, ILogger<ToDoItemService> logger)
        {
            _toDbContext = toDoDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ToDoItemDto> GetAllItems(Guid listId, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            if (toDoList != null)
            {
                var toDoItems = toDoList.ToDoItems.OrderBy(x => x.Position);
                _logger.LogDebug("ToDoItem.GetAllItems() executed!");
                var toDoItemsDto = _mapper.Map<IEnumerable<ToDoItem>, IEnumerable<ToDoItemDto>>(toDoItems);
                return toDoItemsDto;
            }

            return null;
        }

        public ToDoItemDto GetItemById(Guid listId, Guid itemId, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            if (toDoList != null)
            {
                var toDoItem = toDoList.ToDoItems.FirstOrDefault(x => x.Id == itemId);
                _logger.LogDebug("ToDoItem.GetItemById() executed!");
                if (toDoItem != null)
                    return _mapper.Map<ToDoItem, ToDoItemDto>(toDoItem);
            }

            return null;
        }

        public ToDoItemDto CreateItem(Guid listId, ToDoItemDto toDoItemDto, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            int numberOfItems = toDoList.ToDoItems.Count;
            if (toDoList != null)
            {
                var toDoItem = _mapper.Map<ToDoItemDto, ToDoItem>(toDoItemDto);
                toDoList.ToDoItems.Add(toDoItem);             
                toDoItem.Position = numberOfItems++;
                _toDbContext.SaveChanges();
                _logger.LogDebug("ToDoItem.CreateItem() executed!");
                return _mapper.Map<ToDoItem, ToDoItemDto>(toDoItem);
            }
            return null;
        }

        public ToDoItemDto UpdateItem(Guid listId, Guid itemId, ToDoItemDto toDoItemDto, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            if (toDoList != null)
            { 
                var toDoItem = toDoList.ToDoItems.FirstOrDefault(x => x.Id == itemId);
                if (toDoItem != null)
                {
                    var item = _mapper.Map<ToDoItemDto, ToDoItem>(toDoItemDto);
                    toDoItem.Update(item);
                    _toDbContext.SaveChanges();
                    _logger.LogDebug("ToDoItem.UpdateItem() executed!");
                    return _mapper.Map<ToDoItem, ToDoItemDto>(toDoItem);
                }
            }
            return null;
        }

        public bool DeleteItem(Guid listId, Guid itemId, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);

            if (toDoList != null)
            {
                var toDoItem = toDoList.ToDoItems.FirstOrDefault(x => x.Id == itemId);
                if (toDoItem != null)
                {                   
                    toDoList.ToDoItems.Where(x => x.Position > toDoItem.Position).ToList().ForEach(x => x.Position--);
                    
                    toDoList.ToDoItems.Remove(toDoItem);
                    _toDbContext.SaveChanges();
                    _logger.LogDebug("ToDoItem.DeleteItem() executed!");
                    return true;
                }
            }
            return false;
        }

        public EResponse UpdateItemPosition(Guid listId, Guid itemId, int newPosition, string ownerEmail)
        {
            var toDoList = _toDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            if (toDoList != null)
            {
                var toDoItems = toDoList.ToDoItems.ToList();
                var itemToUpdate = toDoList.ToDoItems.FirstOrDefault(x => x.Id == itemId);

                if (itemToUpdate != null)
                {
                    var oldPosition = itemToUpdate.Position;

                    if (newPosition < 0 || newPosition > toDoItems.Count)
                    {
                        return EResponse.BAD_REQUEST;
                    }
                    else if (newPosition < oldPosition)
                    {
                        toDoItems.Where(x => x.Position < oldPosition && x.Position >= newPosition).ToList().ForEach(x => x.Position++);
                    }
                    else
                    {
                        toDoItems.Where(x => x.Position > oldPosition && x.Position <= newPosition).ToList().ForEach(x => x.Position--);
                    }
                    
                    itemToUpdate.Position = newPosition;
                    _toDbContext.SaveChanges();
                    return EResponse.OK;
                }
                
            }          
            return EResponse.NOT_FOUND;
        }

       
    }
}
