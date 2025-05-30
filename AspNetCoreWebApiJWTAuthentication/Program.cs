using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //                 .RequireAuthenticatedUser()
    //                 .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Issuer"],
        ValidAudience = builder.Configuration["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"])),
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
    };
});

var multiSchemePolicy = new AuthorizationPolicyBuilder(
    CookieAuthenticationDefaults.AuthenticationScheme,
    JwtBearerDefaults.AuthenticationScheme)
  .RequireAuthenticatedUser()
  .Build();

builder.Services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy);


var app = builder.Build();

app.UseAuthentication();

app.MapControllers();

app.Run();