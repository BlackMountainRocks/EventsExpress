﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EventsExpress.Core;
using EventsExpress.Core.DTOs;
using EventsExpress.Core.IServices;
using EventsExpress.DTO;
using EventsExpress.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsExpress.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(
            IEventService eventService,
            IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Edit([FromForm]EventDto model)
        {
            var result = model.Id == Guid.Empty 
                ? await _eventService.Create(_mapper.Map<EventDTO>(model))
                : await _eventService.Edit(_mapper.Map<EventDTO>(model));
            if (result.Successed)
            {
                return Ok();
            }
            return BadRequest(result.Message);
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Get(Guid id) => 
            Ok(_mapper.Map<EventDto>(_eventService.EventById(id)));

               
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult All([FromQuery]EventFilterViewModel filter)
        {
            filter.PageSize = 6;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.Events(filter, out int count)),
                    PageViewModel = new PageViewModel(count, filter.Page, filter.PageSize)
                    
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public IActionResult AllForAdmin([FromQuery]EventFilterViewModel filter)
        {
            filter.PageSize = 6;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.Events(filter, out int count)),
                    PageViewModel = new PageViewModel(count, filter.Page, filter.PageSize)
                    
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserToEvent(Guid userId, Guid eventId)
        {
            var res = await _eventService.AddUserToEvent(userId, eventId);
            if (res.Successed)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteUserFromEvent(Guid userId, Guid eventId)
        {

            var res = await _eventService.DeleteUserFromEvent(userId, eventId);
            if (res.Successed)
            {
                return Ok();
            }
            return BadRequest();
        }


        #region Get event-sets for user profile
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult FutureEvents(Guid id, int page=1) {
            var model = new PaginationViewModel();
            model.PageSize = 3;
            model.Page = page;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.FutureEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize)
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        [HttpGet("[action]")]
        public IActionResult PastEvents(Guid id, int page = 1)
        {
            var model = new PaginationViewModel();
            model.PageSize = 3;
            model.Page = page;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.PastEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize)
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        [HttpGet("[action]")]
        public IActionResult EventsToGo(Guid id, int page = 1)
        {
            var model = new PaginationViewModel();
            model.PageSize = 3;
            model.Page = page;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.EventsToGoByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize)
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        [HttpGet("[action]")]
        public IActionResult VisitedEvents(Guid id, int page = 1)
        {
            var model = new PaginationViewModel();
            model.PageSize = 3;
            model.Page = page;
            try
            {
                var viewModel = new IndexViewModel<EventPreviewDto>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewDto>>(_eventService.VisitedEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize)
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }
            #endregion

        }
}