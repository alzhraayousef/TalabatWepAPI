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
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepositry<Customer> CustomerRepositry;

        public CustomerAccountController(UserManager<ApplicationUser> userManager,IConfiguration config, SignInManager<ApplicationUser> signInManager, IRepositry<Customer> CustomerRepositry)
        {
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this.CustomerRepositry= CustomerRepositry;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register(CustomerRegistrationDTO customerDTO)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = customerDTO.Email;
                applicationUser.UserName = customerDTO.UserName;
             //   applicationUser.IsDeleted = false;
                IdentityResult result =await userManager.CreateAsync(applicationUser, customerDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "Customer");//insert row UserRole
                    Customer customer = new Customer();
                    customer.ApplicationUserId = applicationUser.Id;
                    customer.FirstName=customerDTO.FirstName;
                    customer.LastName=customerDTO.LastName;
                    customer.Gender= customerDTO.Gender;
                    customer.IsDeleted = false;
                    CustomerRepositry.create(customer);
                    return Ok("Created Success");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("ModelStateErrors", error.Description);
                    }
                    return BadRequest(ModelState);
                }
                    
            }
            return BadRequest(ModelState);
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(CustomerLoginDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser= await  userManager.FindByEmailAsync(customerDTO.Email);
                if (applicationUser != null && await userManager.CheckPasswordAsync(applicationUser, customerDTO.Password))
                {
                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                    UserClaims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));

                    List<string> roles =(List<string>)await userManager.GetRolesAsync(applicationUser);
                   
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
                        expires:DateTime.Now.AddDays(1),
                        claims:UserClaims,
                        signingCredentials: credentials
                        );

                    // Create Token
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                       // expiration=mytoken.ValidTo,
                       // applicationUserID= applicationUser.Id
                    }) ;

                }

                return BadRequest("Invalid Information");
            }
            return BadRequest(ModelState);
        }
    }
}
