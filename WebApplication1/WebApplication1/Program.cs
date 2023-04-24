using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Web.Http.Cors;
using System.Text;
using System.Web.Http;
using WebApplication1.Models;
using System.Text.Json.Serialization;
using WebApplication1.repo;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
            // Add services to the container.
            builder.Services.AddCors(options => {
                options.AddPolicy("MyPolicy", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            builder.Services.AddControllers().AddJsonOptions(x =>
         x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddDbContext<Context>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("cs"))
       );
           

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Context>();
         ;
            builder.Services.AddScoped<IRepositry<Resturant>, Repositry<Resturant>>();
            builder.Services.AddScoped<IRepositry<City>, Repositry<City>>();
            builder.Services.AddScoped<IRepositry<Category>, Repositry<Category>>();
            builder.Services.AddAuthentication(options =>
            {
                //jwt
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//not valid account
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //parmeter
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIss"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAud"],
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecrytKey"]))//asdZXCZX!#!@342352
                };
            })  //how to check if toke valid or not    
                ;


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = " ITI Projrcy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
             new string[] { }

                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}