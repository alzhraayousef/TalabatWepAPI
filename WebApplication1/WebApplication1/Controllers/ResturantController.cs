using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTO;
using WebApplication1.Helper;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResturantController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepositry<Resturant> ResturantRepositry;

        public ResturantController(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, IRepositry<Resturant> ResturantRepositry)
        {
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this.ResturantRepositry = ResturantRepositry;
           
        }



        [HttpGet("Index")]
        [Authorize]
        public IActionResult Index()
        {
            Resturant r = ResturantRepositry.getbyid(1);
            return Ok(r);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]ResturantRegistrationDTO resturantDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = resturantDTO.Email;
                applicationUser.UserName = resturantDTO.UserName;
                //   applicationUser.IsDeleted = false;
                IdentityResult result = await userManager.CreateAsync(applicationUser, resturantDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "Resturant");//insert row UserRole
                    Resturant resturant = new Resturant();
                    resturant.Image = ImagesHelper.UploadImg(resturantDTO.ImageNameFile, "ResturantIMG");
                    resturant.MinOrderAmmount = resturantDTO.MinOrderAmmount;
                    resturant.DelivaryFee = resturantDTO.DelivaryFee;
                    resturant.DelivaryTime = resturantDTO.DelivaryTime;
                    resturant.IsDeleted = false;
                    resturant.ApplicationUserId = applicationUser.Id;
                    resturant.WorkingHours= resturantDTO.WorkingHours;
                    ResturantRepositry.create(resturant);
                    return Ok(resturant); //"Created Success"
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("ModelStateErrors", error.Description);
                    }
                    return BadRequest(ModelState);
                }

            }
            return BadRequest(ModelState);
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(ResturantLoginDTO resturantDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await userManager.FindByEmailAsync(resturantDTO.Email);
                if (applicationUser != null && await userManager.CheckPasswordAsync(applicationUser, resturantDTO.Password))
                {
                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                    UserClaims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    List<string> roles = (List<string>)await userManager.GetRolesAsync(applicationUser);

                    if (roles != null)
                    {
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
                        expires: DateTime.Now.AddDays(1),
                        claims: UserClaims,
                        signingCredentials: credentials
                        );

                    // Create Token
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                        expiration = mytoken.ValidTo,
                        applicationUserID = applicationUser.Id
                    });

                }

                return BadRequest("Invalid Information");
            }
            return BadRequest(ModelState);
        }
    }
}
