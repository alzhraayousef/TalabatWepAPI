using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userDTO)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.Email = userDTO.Email;
                userModel.UserName = userDTO.UserName;
                IdentityResult result =await userManager.CreateAsync(userModel,userDTO.Password);
                if (result.Succeeded)
                {
                    return Ok("Created Success");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    return BadRequest(ModelState);
                }
                    
            }
            return BadRequest(ModelState);
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel= await  userManager.FindByNameAsync(userDto.UserName);
                if (userModel != null && await userManager.CheckPasswordAsync(userModel,userDto.Password))
                {
                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier,userModel.Id));
                    UserClaims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));

                    List<string> roles =(List<string>)await userManager.GetRolesAsync(userModel);
                   
                    if (roles != null) {
                        foreach (var item in roles)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, item));
                        }
                    }
                    var authSecritKey = 
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecrytKey"]));//asdZXCZX!#!@342352
                    
                    SigningCredentials credentials = 
                        new SigningCredentials(authSecritKey, SecurityAlgorithms.HmacSha256);

                    // Represent Token
                    JwtSecurityToken mytoken = new JwtSecurityToken(
                        issuer: config["JWT:ValidIss"],
                        audience: config["JWT:ValidAud"],
                        expires:DateTime.Now.AddHours(5),
                        claims:UserClaims,
                        signingCredentials: credentials
                        );

                    // Create Token
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                        expiration=mytoken.ValidTo
                    }) ;

                }

                return BadRequest("Invalid Information");
            }
            return BadRequest(ModelState);
        }
    }
}
