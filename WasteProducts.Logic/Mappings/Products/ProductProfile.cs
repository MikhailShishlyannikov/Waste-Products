﻿using System;
using System.Linq;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDB>()
                .ForMember(m => m.Created,
                    opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null));

            CreateMap<ProductDB, Product>()
                .ForMember(m => m.AvgRating,
                    opt => opt.MapFrom(p => p.UserDescriptions.Count > 0 ? Math.Round(p.UserDescriptions.Average(ud => ud.Rating), 2) : (double?)null));
        }
    }
}
