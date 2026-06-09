using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using DiaMate.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, AppDbContext db, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
            _emailService = emailService;
        }
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        private string GenerateVerificationCode()
        {
            return new Random()
                .Next(100000, 999999)
                .ToString();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterNewUser(dtoNewUser user)
        {

            if (ModelState.IsValid)
            {

                string otp = GenerateVerificationCode();
                var appUser = new AppUser()
                {
                    UserName = user.UserName,
                    VerificationCode = otp,
                    VerificationCodeExpiry = DateTime.Now.AddMinutes(10),
                    EmailConfirmed = false,
                    Email=user.Email,
                    Patient = new Patient
                    {
                        DateOfDiagnosis = user.DateOfDiagnosis,
                        DiabetesType = user.DiabetesType,
                        Height = user.Height,
                        Weight = user.Weight,
                        Notes = user.Notes,
                        Person = new Person
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Gender = user.Gender,
                            Email = user.Email,
                            Address = user.Address,
                            Phone = user.Phone,
                            HomePhone = user.HomePhone,
                            DateOfBirth = user.DateOfBirth,
                            ProfileImage = user.ProfileImage
                        }

                    }


                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    await _emailService.SendEmailAsync(
                   user.Email,
                   "DiaMate Verification Code",
                   $@"
                    <h2>Welcome to DiaMate</h2>
                    <p>Your verification code is:</p>
                    <h1>{appUser.VerificationCode}</h1>
                    <p>This code expires in 10 minutes.</p>");


                    return Ok("message: now you have account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
            }
            return BadRequest($"message: {ModelState}");

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogInByUsername(dtoLoginByUsername login)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(login.UserName);
                if (user != null)
                {

                    if (await _userManager.CheckPasswordAsync(user, login.Password))
                    {
                        if (user.EmailConfirmed == true)
                        {
                            var claims = new List<Claim>();
                            // claims.Add(new Claim("tokenNo", "12")); //custom claim ( just for know )
                            claims.Add(new Claim("PatientId", user.PatientId.ToString()));
                            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            var roles = await _userManager.GetRolesAsync(user);
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                            }
                            //signingCredentials
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(
                               claims: claims,
                               issuer: _configuration["JWT:Issuer"],
                               audience: _configuration["JWT:Audience"],
                               expires: DateTime.Now.AddDays(7),
                               signingCredentials: signingCredentials);
                            var _token = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo,

                            };
                            return Ok(_token);
                        }
                        else
                        {
                            return BadRequest("message: Email is not Active");
                        }
                    }
                    else
                    {
                        return Unauthorized("message: Password is invalid");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "username is invalid");
                }
            }
            return BadRequest($"message: {ModelState}");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogInByEmail(dtoLoginByEmail login)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {

                    if (await _userManager.CheckPasswordAsync(user, login.Password))
                    {
                        if (user.EmailConfirmed == true)
                        {
                            var claims = new List<Claim>();
                            // claims.Add(new Claim("tokenNo", "12")); //custom claim ( just for know )
                            claims.Add(new Claim("PatientId", user.PatientId.ToString()));
                            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            var roles = await _userManager.GetRolesAsync(user);
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                            }
                            //signingCredentials
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(
                               claims: claims,
                               issuer: _configuration["JWT:Issuer"],
                               audience: _configuration["JWT:Audience"],
                               expires: DateTime.Now.AddDays(7),
                               signingCredentials: signingCredentials);
                            var _token = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo,

                            };
                            return Ok(_token);
                        }
                        else
                        {
                            return BadRequest("message: Email is not Active");
                        }
                    }
                    else
                    {
                        return Unauthorized("message: Password is invalid");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is invalid");
                }
            }
            return BadRequest($"message: {ModelState}");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyEmail(string email,string code)
        {
            var user =
                await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest("message: User not found");

            if (user.VerificationCodeExpiry > DateTime.Now.AddMinutes(10))
                return BadRequest("message: Code expired");

            if (user.VerificationCode != code)
                return BadRequest("message: Invalid code");

            user.EmailConfirmed = true;
            user.VerificationCode = null;
            user.VerificationCodeExpiry = null;

            await _userManager.UpdateAsync(user);

            return Ok("Email verified successfully");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest("User not found");

            if (user.EmailConfirmed)
                return BadRequest("Email already verified");

            string otp = GenerateVerificationCode();

            user.VerificationCode = otp;
            user.VerificationCodeExpiry = DateTime.Now.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            await _emailService.SendEmailAsync(
                email,
                "DiaMate Verification Code",
                $@"
        <h2>DiaMate Verification</h2>
        <p>Your new verification code is:</p>
        <h1>{user.VerificationCode}</h1>
        <p>This code expires in 10 minutes.</p>");

            return Ok("New verification code sent");
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(dtoChangePassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // get logged in user from token
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return Unauthorized("message: User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {

                return BadRequest(result.Errors);
            }

            return Ok("message: Password changed successfully");
        }
    }
}
