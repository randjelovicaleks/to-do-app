using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoApi.Enums;
using ToDoApi.Utils;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly ToDoDbContext _toDoDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ToDoListService> _logger;

        public ToDoListService(ToDoDbContext context, IMapper mapper, ILogger<ToDoListService> logger)
        {
            _toDoDbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ToDoListDto> GetAllLists(string ownerEmail)
        {
           var toDoLists = _toDoDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).OrderByDescending(x => x.Position);
           _logger.LogDebug("ToDoList.GetAllLists() executed!");
           var toDoListsDto = _mapper.Map<IEnumerable<ToDoList>, IEnumerable<ToDoListDto>>(toDoLists);
           return toDoListsDto;
        }

        public ToDoListDto GetListById(Guid id, string ownerEmail)
        {
            var toDoList = _toDoDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == id);
            _logger.LogDebug("ToDoList.GetListById() executed!");
            if (toDoList != null)
            {
                var toDoListDto = _mapper.Map<ToDoList, ToDoListDto>(toDoList);              
                return toDoListDto;
            }
            return null;
        }

        public ToDoListDto CreateList(ToDoListDto toDoListDto, string ownerEmail)
        {
            var toDoList = _mapper.Map<ToDoListDto, ToDoList>(toDoListDto);
            toDoList.IsReminded = true;
            var numberOfLists = _toDoDbContext.ToDoLists.ToList().Count;
            toDoList.Position = numberOfLists++;
            toDoList.Owner = ownerEmail;

            _toDoDbContext.ToDoLists.Add(toDoList);
            _toDoDbContext.SaveChanges();
            _logger.LogDebug("ToDoList.CreateList() executed!");

            var listDto = _mapper.Map<ToDoList, ToDoListDto>(toDoList);
            return listDto;
        }

        public ToDoListDto UpdateList(Guid id, ToDoListDto toDoListDto, string ownerEmail)
        {
            var toDoList = _toDoDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == id);
            if (toDoList != null)
            {
                var list = _mapper.Map<ToDoListDto, ToDoList>(toDoListDto);
                var updatedList = toDoList.Update(list);

                _toDoDbContext.SaveChanges();
                _logger.LogDebug("ToDoList.UpdateList() executed!");
                return _mapper.Map<ToDoList, ToDoListDto>(updatedList);
            }
            return null;          
        }

        public bool DeleteList(Guid id, string ownerEmail)
        {
            var toDoList = _toDoDbContext.ToDoLists.Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == id);
            if (toDoList != null)
            {             
                _toDoDbContext.ToDoLists.Where(x => x.Position > toDoList.Position && x.Owner == ownerEmail).ToList().ForEach(x => x.Position--);

                _toDoDbContext.ToDoLists.Remove(toDoList);
                _toDoDbContext.SaveChanges();
                _logger.LogDebug("ToDoList.DeleteList() executed!");
                return true;
            }
            return false;
        }

        public IEnumerable<ToDoListDto> SearchLists(string title, string ownerEmail)
        {
            var filteredLists = _toDoDbContext.ToDoLists.Include(x => x.ToDoItems).Where(x => x.Title
                .ToLower()
                .Contains(title.ToLower().Trim()) && x.Owner == ownerEmail)
                .ToList();
            _logger.LogDebug("ToDoList.SearchLists() executed!");
            var filteredListsDto = _mapper.Map<IEnumerable<ToDoList>, IEnumerable<ToDoListDto>>(filteredLists);
            return filteredListsDto;
        }

        public EResponse UpdateListPosition(Guid listId, int newPosition, string ownerEmail)
        {
            var toDoLists = _toDoDbContext.ToDoLists.Where(x => x.Owner == ownerEmail).ToList();
            var listToUpdate = _toDoDbContext.ToDoLists.Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == listId);
            
            if (listToUpdate != null)
            {
                var oldPosition = listToUpdate.Position;

                if (newPosition < 0 || newPosition > toDoLists.Count)
                {
                    return EResponse.BAD_REQUEST;
                }
                else if (newPosition < oldPosition)
                {
                    toDoLists.Where(x => x.Position < oldPosition && x.Position >= newPosition).ToList().ForEach(x => x.Position++);
                }
                else
                {
                    toDoLists.Where(x => x.Position > oldPosition && x.Position <= newPosition).ToList().ForEach(x => x.Position--);
                }
                
                listToUpdate.Position = newPosition;
                _toDoDbContext.SaveChanges();
                return EResponse.OK;
            }
            return EResponse.NOT_FOUND;
        }

        public ShareToDoList ShareToDoList(Guid id, string ownerEmail)
        {
            var toDoList = _toDoDbContext.ToDoLists.Where(x => x.Owner == ownerEmail).FirstOrDefault(x => x.Id == id);
            if (toDoList != null)
            {
                ShareToDoList shareToDoList = new ShareToDoList();
                shareToDoList.ToDoListId = id;
                shareToDoList.ExpiryDateTime = DateTime.Now.AddHours(2);
                _toDoDbContext.ShareToDoLists.Add(shareToDoList);
                _toDoDbContext.SaveChanges();
                return shareToDoList;
            }
            return null;
        }

        public ToDoListDto GetSharedToDoList(Guid shareToDoListId)
        {
            var shareToDoList = _toDoDbContext.ShareToDoLists.Where(x => x.ExpiryDateTime > DateTime.Now).FirstOrDefault(x => x.Id == shareToDoListId);
            if (shareToDoList != null)
            { 
                var toDoList = _toDoDbContext.ToDoLists.Include(x => x.ToDoItems).FirstOrDefault(x => x.Id == shareToDoList.ToDoListId);
                if (toDoList != null)
                { 
                    var sharedToDoListDto = _mapper.Map<ToDoList, ToDoListDto>(toDoList);
                    return sharedToDoListDto;
                }
            }
            return null;
        }
    }
}
