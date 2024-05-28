using FlightAPI.Data;
using FlightAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.OpenApi.Models;
using FlightAPI.Profiles;
using FlightAPI.Services.Interfaces;
using FlightAPI.Services.Implementations;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Repositories.Implementations;
using FlightAPI.Utility;
using FlightAPI.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>(); 
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IFlightDapperRepository, FlightDapperRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IPlaneRepository, PlaneRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IFlightCommandService, FlightCommandService>();
builder.Services.AddScoped<IFlightQueryService, FlightQueryService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JwtSettings:ValidAudience"],
        ValidIssuer = configuration["JwtSettings:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger documentation to add support for JWT Bearer
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
           new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!(await roleManager.RoleExistsAsync(Constants.Role_Admin)))
    {
        await roleManager.CreateAsync(new IdentityRole(Constants.Role_Admin));
    }

    if (!(await roleManager.RoleExistsAsync(Constants.Role_Customer)))
    {
        await roleManager.CreateAsync(new IdentityRole(Constants.Role_Customer));
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
