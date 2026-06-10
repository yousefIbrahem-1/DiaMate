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
      "Welcome to DiaMate",
      $@"
<!DOCTYPE html>
<html>
<body style='margin:0;
             padding:0;
             background:#f4f7fb;
             font-family:Segoe UI,Arial,sans-serif;'>

<div style='max-width:650px;
            margin:40px auto;
            background:#ffffff;
            border-radius:20px;
            overflow:hidden;
            box-shadow:0 5px 25px rgba(0,0,0,0.08);'>

    <div style='background:linear-gradient(135deg,#7dd3fc,#38bdf8);
                padding:40px;
                text-align:center;'>

        <img src='https://raw.githubusercontent.com/yousefIbrahem-1/DiaMate/main/Logo.png'
             width='120'
             alt='DiaMate Logo'
             style='margin-bottom:15px;' />

        <h1 style='color:white;
                   margin:0;'>
            DiaMate
        </h1>

        <p style='color:#e0f2fe;
                  margin-top:10px;'>

            Smart Diabetes Management Platform

        </p>

    </div>

    <div style='padding:40px;'>

        <h2 style='color:#1f2937;'>

            Welcome {user.FirstName} 👋

        </h2>

        <p style='color:#6b7280;
                  line-height:1.8;'>

            Thank you for joining DiaMate.

            To complete your registration,
            please verify your email address using
            the verification code below.

        </p>

        <div style='margin:35px 0;
                    text-align:center;'>

            <div style='display:inline-block;
                        background:#f0f9ff;
                        border:2px dashed #38bdf8;
                        border-radius:15px;
                        padding:18px 35px;'>

                <span style='font-size:28px;
                             font-weight:600;
                             letter-spacing:6px;
                             color:#0284c7;'>

                    {appUser.VerificationCode}

                </span>

            </div>

        </div>

        <div style='background:#eff6ff;
                    border-left:5px solid #38bdf8;
                    padding:15px;
                    border-radius:10px;'>

            <strong style='color:#0284c7;'>

                Important:

            </strong>

            This verification code will expire in
            10 minutes.

        </div>

        <p style='margin-top:25px;
                  font-size:13px;
                  color:#9ca3af;
                  text-align:center;'>

            If you did not create a DiaMate account,
            please ignore this email.

        </p>

        <div style='margin-top:35px;
                    padding-top:20px;
                    border-top:1px solid #e5e7eb;
                    text-align:center;'>

            <p style='color:#6b7280;
                      margin-bottom:5px;'>

                Best Regards,

            </p>

            <h3 style='margin:0;
                       color:#0284c7;'>

                DiaMate Team

            </h3>

            <p style='color:#9ca3af;
                      font-size:13px;'>

                Smart Diabetes Management Platform

            </p>

        </div>

    </div>

</div>

</body>
</html>");


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

            if (user.EmailConfirmed)
                return BadRequest("Email is already verified.");

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
             "DiaMate - New Verification Code",
             $@"
<!DOCTYPE html>
<html>
<body style='margin:0;
             padding:0;
             background:#f4f7fb;
             font-family:Segoe UI,Arial,sans-serif;'>

<div style='max-width:650px;
            margin:40px auto;
            background:#ffffff;
            border-radius:20px;
            overflow:hidden;
            box-shadow:0 5px 25px rgba(0,0,0,0.08);'>

    <div style='background:linear-gradient(135deg,#7dd3fc,#38bdf8);
                padding:40px;
                text-align:center;'>

        <img src='https://raw.githubusercontent.com/yousefIbrahem-1/DiaMate/main/Logo.png'
             width='120'
             alt='DiaMate Logo'
             style='margin-bottom:15px;' />

        <h1 style='color:white;
                   margin:0;'>
            DiaMate
        </h1>

        <p style='color:#e0f2fe;
                  margin-top:10px;'>
            Smart Diabetes Management Platform
        </p>

    </div>

    <div style='padding:40px;'>

        <h2 style='color:#1f2937;'>
            New Verification Code Sent 🔄
        </h2>

        <p style='color:#6b7280;
                  line-height:1.8;'>

            You requested a new verification code.

            Please use the code below to verify
            your DiaMate account.

        </p>

        <div style='margin:35px 0;
                    text-align:center;'>

            <div style='display:inline-block;
                        background:#f0f9ff;
                        border:2px dashed #38bdf8;
                        border-radius:15px;
                        padding:18px 35px;'>

                <span style='font-size:28px;
                             font-weight:600;
                             letter-spacing:6px;
                             color:#0284c7;'>

                    {user.VerificationCode}

                </span>

            </div>

        </div>

        <div style='background:#eff6ff;
                    border-left:5px solid #38bdf8;
                    padding:15px;
                    border-radius:10px;'>

            <strong style='color:#0284c7;'>
                Important:
            </strong>

            This verification code will expire in
            10 minutes. Any previous verification
            code is no longer valid.

        </div>

        <p style='margin-top:25px;
                  font-size:13px;
                  color:#9ca3af;
                  text-align:center;'>

            If you didn't request a new code,
            please ignore this email.

        </p>

        <div style='margin-top:35px;
                    padding-top:20px;
                    border-top:1px solid #e5e7eb;
                    text-align:center;'>

            <p style='color:#6b7280;
                      margin-bottom:5px;'>

                Best Regards,

            </p>

            <h3 style='margin:0;
                       color:#0284c7;'>

                DiaMate Team

            </h3>

            <p style='color:#9ca3af;
                      font-size:13px;'>

                Smart Diabetes Management Platform

            </p>

        </div>

    </div>

</div>

</body>
</html>");

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
