﻿using EventsExpress.Core.DTOs;
using EventsExpress.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsExpress.Core.IServices
{
    public interface IEventService
    {
        bool Exists(Guid id);

        Task<OperationResult> Create(EventDTO eventDTO);
        Task<OperationResult> Edit(EventDTO e);
        Task<OperationResult> Delete(Guid eventId);
        Task<OperationResult> BlockEvent(Guid eID);
        Task<OperationResult> UnblockEvent(Guid eId);

        EventDTO EventById(Guid eventId);       

        IEnumerable<EventDTO>  Events(EventFilterViewModel model, out int count);
        IEnumerable<EventDTO> EventsForAdmin(EventFilterViewModel model, out int count);

        IEnumerable<EventDTO> FutureEventsByUserId(Guid userId);
        IEnumerable<EventDTO> PastEventsByUserId(Guid userId);
        IEnumerable<EventDTO> VisitedEventsByUserId(Guid userId);
        IEnumerable<EventDTO> EventsToGoByUserId(Guid userId);
        IEnumerable<EventDTO> GetEvents(List<Guid> eventIds);
        Task<OperationResult> AddUserToEvent(Guid userId, Guid eventId);
        Task<OperationResult> DeleteUserFromEvent(Guid userId, Guid eventId);
        
    }
}
