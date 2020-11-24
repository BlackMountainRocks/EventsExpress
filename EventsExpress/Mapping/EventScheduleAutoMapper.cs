﻿using System;
using System.Linq;
using AutoMapper;
using EventsExpress.Core.DTOs;
using EventsExpress.Core.Extensions;
using EventsExpress.Db.Entities;
using EventsExpress.DTO;

namespace EventsExpress.Mapping
{
    public class EventScheduleAutoMapper : Profile
    {
        public EventScheduleAutoMapper()
        {
            CreateMap<EventSchedule, EventScheduleDTO>()
           .ForMember(dest => dest.Event, opts => opts.MapFrom(src => new EventDTO
           {
               Id = src.Event.Id,
               Title = src.Event.Title,
               DateTo = src.Event.DateTo,
               DateFrom = src.Event.DateFrom,
               OwnerId = src.Event.OwnerId,
               PhotoBytes = src.Event.Photo,
           }));

            CreateMap<EventScheduleDTO, EventSchedule>().ReverseMap();

            CreateMap<EventScheduleDTO, EventScheduleDto>()
                .ForMember(dest => dest.Event, opts => opts.MapFrom(src => new EventDTO
                {
                    Id = src.Event.Id,
                    Title = src.Event.Title,
                    DateTo = src.Event.DateTo,
                    DateFrom = src.Event.DateFrom,
                    OwnerId = src.Event.OwnerId,
                    PhotoUrl = src.Event.PhotoBytes.Img.ToRenderablePictureString(),
                }));

            CreateMap<EventScheduleDto, EventScheduleDTO>()
                .ForMember(dest => dest.Event, opts => opts.MapFrom(src => new EventDTO
                {
                    Id = src.Event.Id,
                    Title = src.Event.Title,
                    DateTo = src.Event.DateTo,
                    DateFrom = src.Event.DateFrom,
                    OwnerId = src.Event.OwnerId,
                    PhotoUrl = src.Event.PhotoBytes.Img.ToRenderablePictureString(),
                }));

            CreateMap<EventDTO, EventScheduleDTO>()
                .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.LastRun, opts => opts.MapFrom(src => src.DateTo))
                .ForMember(dest => dest.NextRun, opts => opts.MapFrom(src => DateTimeExtensions.AddDateUnit(src.Periodicity, src.Frequency, src.DateTo)))
                .ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.CreatedDateTime, opts => opts.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ModifiedBy, opts => opts.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.ModifiedDateTime, opts => opts.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => true));
        }
    }
}