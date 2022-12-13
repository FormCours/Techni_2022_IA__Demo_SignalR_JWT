using Demo_ASP__SignalR_JWT.DTO;
using Demo_ASP__SignalR_JWT.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_ASP__SignalR_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenManager _TokenManager;

        public AuthController(TokenManager tokenManager)
        {
            _TokenManager = tokenManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthTokenDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthLoginDTO login)
        {
            if(login.Password != "Test1234=")
            {
                ModelState.AddModelError("credentials", "Bad credentials !");
                return ValidationProblem();
            }

            string token = _TokenManager.GenerateToken(login.Username);

            return Ok(new AuthTokenDTO()
            {
                Token= token
            });
        }

    }
}
