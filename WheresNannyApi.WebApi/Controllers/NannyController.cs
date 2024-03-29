﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NannyController : ControllerBase
    {
        private readonly INannyService _nannyService;
        public NannyController(INannyService nannyService)
        {
            _nannyService = nannyService;
        }

        [HttpGet("GetAllServices/{userId}/{pageIndex}")]
        [Authorize]
        public IActionResult GetAllServices([FromRoute] int userId, int pageIndex)
        {
            try
            {
                var services = _nannyService.GetAllNannyServices(userId, pageIndex);

                if (services == null) return NotFound("Nenhum serviço foi encontrado.");

                return Ok(services);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDashboardInformation/{userId}")]
        [Authorize]
        public IActionResult GetNannyDashboardInformationDto(int userId)
        {
            try
            {
                var nannyDashboardInformation = _nannyService.GetNannyDashboardInformationDto(userId);

                if (nannyDashboardInformation == null) return NotFound("Usuário não foi encontrado.");

                return Ok(nannyDashboardInformation);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEarnsByMonth/{month}/{userId}")]
        [Authorize]
        public IActionResult GetEarnsByMonth([FromRoute] int month, int userId)
        {
            try
            {
                var earnByMonth = _nannyService.GetEarnsByMonth(month, userId);

                return Ok(earnByMonth);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
