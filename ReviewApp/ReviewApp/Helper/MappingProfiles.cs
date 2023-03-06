﻿using AutoMapper;
using ReviewApp.Dto;
using ReviewApp.Models;

namespace ReviewApp.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<PokemonDto, Pokemon>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country>();
        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerDto, Owner>();
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();
        CreateMap<ReviewerDto, Reviewer>();
        CreateMap<Reviewer, ReviewerDto>();
    }
}
