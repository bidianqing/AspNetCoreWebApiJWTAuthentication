using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AspNetCoreWebApiJWTAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Microsoft.Net.Http.Headers.HeaderNames.au
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Issuer"],
                    ValidAudience = Configuration["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"])),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    //NameClaimType="userId"
                };

                options.Events = new JwtBearerEvents();
                // 当没有鉴权或鉴权失败 http请求以401状态吗响应  此时response body 没有值
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

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
