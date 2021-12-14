using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Clientes.DAL;
using Clientes.DAL.Entities;
using Clientes.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.Extensions.Options;
using Clientes.API.MailServices.Interfaces;
using Clientes.API.MailServices.DTOs;

namespace Clientes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IConfiguration _config;

        private readonly IOptions<EmailOptionsDTO> _emailOptions;

        private readonly IEmail _email;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config,
         IOptions<EmailOptionsDTO> emailOptions, IEmail email)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailOptions = emailOptions;
            _email = email;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            //check if user is NULL, to avoid exceptions
            if(user == null)
            {
                return BadRequest();
            }
            //last variable false to not lockout after failed login attempts
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(new {
                result = result,
                token = JwtTokenGenerator(user)
            });
        }

        // POST api/auth/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //get user from email
            var user = await _userManager.FindByEmailAsync(model.Email);
            //confirm user exists and if confirmed email
            if (user != null || user.EmailConfirmed)
            {
                //copied from Create under UserController
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var changePasswordUrl = Request.Headers["changePasswordUrl"];

                var uriBuilder = new UriBuilder(changePasswordUrl);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["token"] = token;
                query["userid"] = user.Id;
                uriBuilder.Query = query.ToString();
                var urlString = uriBuilder.ToString();

                var emailBody = $"Cambie su contraseña haciendo click en el siguiente link <br>{urlString}";
                await _email.Send(model.Email, emailBody, _emailOptions.Value);

                return Ok();
            }
            return Unauthorized();
        }

        // POST api/auth/changepassword
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(model.Token), model.Password);

            if(resetPasswordResult.Succeeded)
            {
                return Ok();
            }
            return Unauthorized();}
        
        // Post api/auth/confirm-email
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var confirm = await _userManager.ConfirmEmailAsync(user, Uri.UnescapeDataString(model.Token));

            if(confirm.Succeeded)
            {
                return Ok();
            }
            return Unauthorized();
        }

        //jwt token generation method
        private string JwtTokenGenerator(User userInfo)  
        {  
            //include user and userid in the token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                new Claim(ClaimTypes.Name, userInfo.UserName)
            };
            //token validation
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8
             .GetBytes(_config.GetSection("AppSettings:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }   
    }
}
