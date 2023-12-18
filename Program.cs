using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Marlin.sqlite.Data;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Microsoft.Extensions.Hosting;
using k8s;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var origins = "_origins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_origins",
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader()
                              .AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{


    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWT",
        Version = "v1",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type =SecuritySchemeType.ApiKey,
        Scheme="Bearer",
        BearerFormat ="JWT",
        In =ParameterLocation.Header,
        Description="test"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string []{}
        }
    });
});
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();


// Update the appsettings.json file with the actual secret key
var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
var appSettingsJson = File.ReadAllText(appSettingsPath);
dynamic appSettings = Newtonsoft.Json.JsonConvert.DeserializeObject(appSettingsJson);

// JWT authentication configuration
var issuer = appSettings["JWT"]["Issuer"].Value;
var audience = appSettings["JWT"]["Audience"].Value;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("T2Nkz8rlfyZrWvPQxGcL7/8QZJ0ZHX6dOPBbJSEbI+8="))
    };
});

builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new NullableDecimalConverter());
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        // Use the path to the certificate.pfx file and its password within the secret.
        listenOptions.UseHttps("/mnt/secrets/marlin-secret/certificate.pfx", "marlindemo");
    });
});





var app = builder.Build();
    app.UseCors(origins);
    app.UseHttpsRedirection();
   
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseSwagger();
    app.UseSwaggerUI();
   

app.Run();

