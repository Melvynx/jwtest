using System.Text;
using Jwtest;
using Jwtest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {NamingStrategy = new CamelCaseNamingStrategy()};
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    }
);

builder.Services.AddDbContext<JwtContext>(options =>
    options.UseSqlite("Data Source='/Users/Shared/db/jwtest.db'"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { })
    .AddEntityFrameworkStores<JwtContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings")["Secret"]);

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

app.UseCors(corsSettings => corsSettings
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// custom jwt auth middleware
// app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();