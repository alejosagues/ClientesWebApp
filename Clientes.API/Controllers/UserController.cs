using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Clientes.DAL;
using Clientes.DAL.Entities;
using Clientes.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Clientes.API.MailServices.DTOs;
using Clientes.API.MailServices.Interfaces;

namespace Clientes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly IOptions<EmailOptionsDTO> _emailOptions;

        private readonly IEmail _email;

        public UsersController(UserManager<User> userManager, IOptions<EmailOptionsDTO> emailOptions
        , IEmail email)
        {
            _emailOptions = emailOptions;
            _email = email;
            _userManager = userManager;
        }

        // POST api/user/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result);
            }

            //send email confirmation

            //generate unique token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //pass header for confirmation (from SPA)
            var confirmEmailUrl = Request.Headers["confirmEmailUrl"];

            //build the confirmation token+userid
            var uriBuilder = new UriBuilder(confirmEmailUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = user.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            //attach to the email body
            var emailBody = $"Por favor confirme su email haciendo click en el siguiente link <br>{urlString}";
            //wait for the email to be sent before generating a response
            await _email.Send(model.Email, emailBody, _emailOptions.Value);

            return Ok(result);
        }
    }
}
