using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Mapping;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.Auth;
using WorkoutTracker.Api.Services.TrainingSessions;

namespace WorkoutTracker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;   
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = builder.Configuration["Jwt:Key"];
                if (string.IsNullOrWhiteSpace(jwtKey))
                {
                    throw new InvalidOperationException("JWT key is missing in configuration. Set 'Jwt:Key' in appsettings or environment variables.");
                }

                options.SaveToken = true; 
                options.RequireHttpsMetadata = false; // dev = false, prod = true
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true, 

                    ValidIssuer = builder.Configuration["Jwt:Issuer"], 
                    ValidAudience = builder.Configuration["Jwt:Audience"], 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) 
                };
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WorkoutTracker API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header, 
                    Description = "Enter JWT token with the word 'Bearer ' before. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization", 
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,                                                   
                    Scheme = "Bearer" 
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement 
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddTransient<ITrainingSessionsService, TrainingSessionService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
