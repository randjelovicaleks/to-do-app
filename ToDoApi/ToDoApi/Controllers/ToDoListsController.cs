using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoApi.Enums;
using ToDoApi.Services;
using ToDoCore;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Controllers
{
    [Route("api/to-do-lists")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;
        private readonly IToDoItemService _toDoItemService;

        public ToDoListsController(IToDoListService toDoListService, IToDoItemService toDoItemService)
        {
            _toDoListService = toDoListService;
            _toDoItemService = toDoItemService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GetEmailFromToken() 
        {
            return User.Claims.FirstOrDefault(c => c.Type == "https://to-do-app.com/email")?.Value;
        }

        [HttpGet]
        [Authorize("read:to-do-lists")]
        public IActionResult GetAllLists()
        {
            var toDoListsDto = _toDoListService.GetAllLists(GetEmailFromToken());
            return Ok(toDoListsDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ToDoList))]
        [Authorize("read:to-do-list")]
        public IActionResult GetListById(Guid id)
        {
            var toDoListDto = _toDoListService.GetListById(id, GetEmailFromToken());
            if (toDoListDto != null)
                return Ok(toDoListDto);

            return NotFound();
        }

        [HttpPost]
        [Authorize("create:to-do-list")]
        public IActionResult CreateList([FromBody] ToDoListDto toDoListDto)
        {

            var newToDoListDto = _toDoListService.CreateList(toDoListDto, GetEmailFromToken());
            return CreatedAtAction(nameof(GetListById), new { id = newToDoListDto.Id }, newToDoListDto);
        }

        [HttpPut("{id}")]
        [Authorize("update:to-do-list")]
        public IActionResult UpdateList(Guid id, [FromBody] ToDoListDto toDoListDto)
        {
            var updatedList = _toDoListService.UpdateList(id, toDoListDto, GetEmailFromToken());
            if (updatedList != null)
                return Ok(updatedList);
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize("delete:to-do-list")]
        public IActionResult DeleteList(Guid id)
        {
            if (_toDoListService.DeleteList(id, GetEmailFromToken()))
                return Ok();

            return NotFound();
        }

        [HttpGet("search/{title}")]
        [Authorize("read:to-do-lists")]
        public IActionResult Search(string title)
        {
            var filteredListsDto = _toDoListService.SearchLists(title, GetEmailFromToken());
            return Ok(filteredListsDto);
        }

        [HttpGet("{listId}/to-do-items")]
        [Authorize("read:to-do-items")]
        public IActionResult GetAllItems(Guid listId)
        {
            var toDoItemsDto = _toDoItemService.GetAllItems(listId, GetEmailFromToken());
            return Ok(toDoItemsDto);
        }

        [HttpGet("{listId}/to-do-items/{itemId}")]
        [Authorize("read:to-do-item")]
        public IActionResult GetItemById(Guid listId, Guid itemId)
        {
            var toDoItemDto = _toDoItemService.GetItemById(listId, itemId, GetEmailFromToken());
            if (toDoItemDto != null)
                return Ok(toDoItemDto);

            return NotFound();
        }

        [HttpPost("{listId}/to-do-items")]
        [Authorize("create:to-do-item")]
        public IActionResult CreateItem(Guid listId, [FromBody] ToDoItemDto toDoItemDto)
        {
            var newToDoItemDto = _toDoItemService.CreateItem(listId, toDoItemDto, GetEmailFromToken());
            if (newToDoItemDto != null)
                return CreatedAtAction(nameof(GetItemById), new { listId = listId, itemId = newToDoItemDto.Id }, newToDoItemDto);

            return BadRequest();
        }

        [HttpPut("{listId}/to-do-items/{itemId}")]
        [Authorize("update:to-do-item")]
        public IActionResult UpdateItem(Guid listId, Guid itemId, [FromBody] ToDoItemDto toDoItemDto)
        {
            var updatedItem = _toDoItemService.UpdateItem(listId, itemId, toDoItemDto, GetEmailFromToken());

            if (updatedItem != null)
                return Ok(updatedItem);

            return BadRequest();
        }

        [HttpDelete("{listId}/to-do-items/{itemId}")]
        [Authorize("delete:to-do-item")]
        public IActionResult DeleteItem(Guid listId, Guid itemId)
        {
            if (_toDoItemService.DeleteItem(listId, itemId, GetEmailFromToken()))
                return Ok();

            return NotFound();
        }

        [HttpPut("{listId}/{newPosition}")]
        [Authorize("update-position:to-do-list")]
        public IActionResult UpdateListPosition(Guid listId, int newPosition)
        {
            var responseType = _toDoListService.UpdateListPosition(listId, newPosition, GetEmailFromToken());
            if (responseType.Equals(EResponse.OK))
            {
                return Ok();
            }
            else if (responseType.Equals(EResponse.NOT_FOUND))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }         
        }

        [HttpPut("{listId}/to-do-items/{itemId}/{newPosition}")]
        [Authorize("update:to-do-item")]
        public IActionResult UpdateItemPosition(Guid listId, Guid itemId, int newPosition)
        {
            var responseType = _toDoItemService.UpdateItemPosition(listId, itemId, newPosition, GetEmailFromToken());
            if (responseType.Equals(EResponse.OK))
            {
                return Ok();
            }
            else if (responseType.Equals(EResponse.NOT_FOUND))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}")]
        [Authorize("share:to-do-list")]
        public IActionResult ShareToDoList(Guid id)
        {
            var shareToDoList = _toDoListService.ShareToDoList(id, GetEmailFromToken());
            if (shareToDoList != null)
            {
                return Ok("http://localhost:4200/to-do-list/share/" + shareToDoList.Id);

            }
            return BadRequest();
        }

        [HttpGet("{shareToDoListId}/share")]
        [AllowAnonymous]
        public IActionResult GetSharedToDoList(Guid shareToDoListId)
        {
            var sharedToDoListDto = _toDoListService.GetSharedToDoList(shareToDoListId);
            if (sharedToDoListDto != null)
            {
                return Ok(sharedToDoListDto);
            }
            return NotFound();
        }

    }
}
