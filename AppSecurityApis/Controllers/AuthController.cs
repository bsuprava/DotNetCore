using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJwtTokenAuthentication;
using MyJwtTokenAuthentication.Interfaces;
using MyJwtTokenAuthentication.Models;
using System.Security.Claims;

namespace AppSecurityApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IJwtTokenValidator _jwtTokenValidator;
        public AuthController(
            IJwtTokenGenerator jwtTokenGenerator,
            IJwtTokenValidator jwtTokenValidator) 
        { 
            _jwtTokenGenerator = jwtTokenGenerator; 
            _jwtTokenValidator = jwtTokenValidator;
        }

        [HttpPost]
        [Route("GetToken")]
        public IActionResult GetToken([FromBody] GenerateTokenDto generateTokenDto)
        {
            try
            {
                var token = _jwtTokenGenerator.GenerateToken(generateTokenDto.UserName, generateTokenDto.Role, generateTokenDto.ExpiryTime);
                if (!string.IsNullOrEmpty(token))
                    return Ok(token);
                else return BadRequest();
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        [Route("ValidateToken")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            try
            {
                // Validate the token
                ClaimsPrincipal? principal = _jwtTokenValidator.ValidateToken(token);

                if (principal != null)
                {
                    // Access claims from the token
                    foreach (var claim in principal.Claims)
                    {
                        Console.WriteLine($"{claim.Type}: {claim.Value}");
                    }
                    return Ok("Token is valid!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Token is invalid!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
