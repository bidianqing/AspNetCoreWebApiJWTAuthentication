using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreWebApiJWTAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Issuer"],
                    ValidAudience = Configuration["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"]))
                };

                options.Events = new JwtBearerEvents();
                //options.Events.OnChallenge = context =>
                //{
                //    //Exception exception = context.AuthenticateFailure;
                //    //if (exception != null)
                //    //{
                //    //    return Task.CompletedTask;
                //    //}

                //    context.HandleResponse();

                //    var payload = new JObject
                //    {
                //        ["success"] = false,
                //        ["message"] = "无效授权",
                //        ["payload"] = null
                //    };

                //    context.Response.ContentType = "application/json";
                //    context.Response.StatusCode = 401;

                //    return context.Response.WriteAsync(payload.ToString());
                //};
                //options.Events.OnAuthenticationFailed = context =>
                //{
                //    var payload = new JObject
                //    {
                //        ["success"] = false,
                //        ["message"] = "授权失败",
                //        ["data"] = null
                //    };

                //    context.Response.OnStarting(async () =>
                //    {
                //        context.Response.StatusCode = 401;
                //        context.Response.ContentType = "application/json";
                //        await context.Response.WriteAsync(payload.ToString());
                //    });

                //    return Task.CompletedTask;
                //};
            });

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
