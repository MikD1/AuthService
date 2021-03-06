﻿using System.Net;
using System.Threading.Tasks;
using AuthService.Model;
using AuthService.WebApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthService.WebApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> Register([FromBody] AccountDto dto)
        {
            if (dto?.Login is null || dto.Password is null)
            {
                throw new ServiceException("Invalid argument.", HttpStatusCode.BadRequest);
            }

            User user = await _userService.Register(dto.Login, dto.Password);
            return Ok(new UserDto(user));
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Authenticate([FromBody] AccountDto dto)
        {
            if (dto?.Login is null || dto.Password is null)
            {
                throw new ServiceException("Invalid argument.", HttpStatusCode.BadRequest);
            }

            User user = await _userService.Authenticate(dto.Login, dto.Password);
            return Ok(new UserWithTokenDto(user));
        }

        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
    }
}
