using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Dtos;
using ToDoCore.Models;

namespace ToDoApi.Mapping
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<ToDoList, ToDoListDto>();
            CreateMap<ToDoItem, ToDoItemDto>();
        }
    }
}
