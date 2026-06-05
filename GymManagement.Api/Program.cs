using GymManagement.Api.Middlewares;
using GymManagement.Api.Models;
using GymManagement.Application;
using GymManagement.Application.Settings;
using GymManagement.Infrastructure;
using GymManagement.Persistence;
using GymManagement.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        var response = new ErrorResponse
        {
            Message = "Validation failed",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Management API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token like this: Bearer your-token-here",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Project Services
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

// JWT Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

var jwtSettings = builder.Configuration
    .GetSection(nameof(JwtSettings))
    .Get<JwtSettings>();

if (jwtSettings is null)
{
    throw new InvalidOperationException("JwtSettings section is missing in appsettings.json");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
{
    throw new InvalidOperationException("JwtSettings:Secret is missing in appsettings.json");
}

// Authentication - JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Message = "Unauthorized"
                };

                response.Errors.Add("You are not authenticated");

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Message = "Forbidden"
                };

                response.Errors.Add("You do not have permission to access this resource");

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        };
    });

// Authorization
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// Swagger Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Roles and Admin Permissions
await IdentitySeeder.SeedRolesAsync(app.Services);

app.Run();