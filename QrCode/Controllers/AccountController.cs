using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QrCode.API.DTOs;
using QrCode.API.Services;
using QrCode.DB.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QrCode.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration config;
        private readonly IEmailServices emailSender;

        public AccountController(UserManager<AppUser> userManager
                                , IConfiguration config
                                , IEmailServices emailSender)
        {
            this.userManager = userManager;
            this.config = config;
            this.emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            AppUser appUser = await userManager.FindByNameAsync(registerDTO.Username);
            if (appUser != null)
                return BadRequest("This UserName is already exists");

            appUser = await userManager.FindByEmailAsync(registerDTO.Email);
            if (appUser != null)
                return BadRequest("This Email is already exists");

            appUser = new()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                EmailConfirmed = false
            };

            IdentityResult result =
                await userManager.CreateAsync(appUser, registerDTO.Password);

            if (result.Succeeded)
            {
                string body = await EmailBody(appUser);
                EmailDTO emailDTO = new()
                {
                    To = appUser.Email,
                    Body = body
                };
                emailSender.SendEmail(emailDTO);

                return Ok("Please verify your account by mail just sent");

            }
            return BadRequest(result.Errors);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest("Invalid email confirmation");

            AppUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid email confirmation");

            IdentityResult result = await userManager.ConfirmEmailAsync(user, code);
            string home = config["JWT:Audience"];
            string htmlResponse = "";
            if (result.Succeeded)
            {
                htmlResponse = $"<html><body>Email confirmed successfully. Thank you. <a href=\"{home}\">Go to Home Page</a></body></html>";
                return Content(htmlResponse, "text/html");
            }

            htmlResponse = $"<html><body>An error occurred. Please try again later. <a href=\"{home}\">Go to Home Page</a></body></html>";

            return Content(htmlResponse, "text/html");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            AppUser user = await userManager.FindByNameAsync(loginDTO.username);

            if (user != null && !user.EmailConfirmed)
                return Unauthorized("Please verify your account");

            if (user != null && userManager.CheckPasswordAsync(user, loginDTO.password).Result)
                return Ok(await GenerateToken(user));

            return BadRequest("Invalid account");
            
        }
        //}
        private async Task<string> EmailBody(AppUser appUser)
        {
            string code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string emailBody = $@"
                    <html>
                    <body>
                        <p>Dear {appUser.UserName},</p>
                        <p>Thank you for registering with our website. To complete your registration, please click the following link:</p>
                        <p><a href='#URL#'>Confirm Your Email</a></p>
                        <p>If you didn't request this email, you can safely ignore it.</p>
                        <p>Best regards,</p>
                        <p>QrCode Generator Team</p>
                    </body>
                    </html>";

            string callBackUrl = Request.Scheme + "://" + Request.Host +
               Url.Action("ConfirmEmail", "Account", new { userId = appUser.Id, code });

            return emailBody.Replace("#URL#",
                        System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callBackUrl));
        }
        private async Task<string> GenerateToken(AppUser user)
        {
            List<Claim> claims = new ()
             {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            IList<string> roles = await userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(config["JWT:Key"]);

            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            );

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}

