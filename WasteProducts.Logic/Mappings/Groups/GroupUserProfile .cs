﻿using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupUserProfile : Profile
    {
        public GroupUserProfile()
        {
            CreateMap<GroupUser, GroupUserDB>()
                .ForMember(x => x.GroupId, y => y.MapFrom(z => z.GroupId))
                .ForMember(x => x.Modified, y => y.Ignore())
                .ForMember(x => x.RightToCreateBoards, y => y.Ignore())
                .ForMember(x => x.IsConfirmed, y => y.Ignore())
                .ReverseMap();
        }
    }
}
