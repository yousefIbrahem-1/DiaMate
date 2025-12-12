using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration,AppDbContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
        }
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

      

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterNewUser(dtoNewUser user)
        {
           
            if (ModelState.IsValid)
            {
               
                var appUser = new AppUser()
                {
                    UserName = user.UserName,
                    Patient = new Patient
                    {
                        Weight = user.Weight,
                        Notes = user.Notes,
                        Person = new Person
                        {
                            FirstName = user.FirstName,
                            SecondName = user.SecondName,
                            ThirdName = user.ThirdName,
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
                  
                    return Ok();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(login.UserName);
                if (user != null)
                {
                   
                    if (await _userManager.CheckPasswordAsync(user, login.Password))
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
                        return Unauthorized("Password is invalid");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "username is invalid");
                }
            }
            return BadRequest(ModelState);
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
                return Unauthorized("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully");
        }


        [HttpGet]
        public IActionResult Get() => Ok("API works");
    }
}
