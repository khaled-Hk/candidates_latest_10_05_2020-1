using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models;
using VueCliMiddleware;

namespace Dashboard
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
            //Handle XSRF Name for Header
            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddDbContext<CandidatesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Candidate")));
         
          

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();

            //  services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)


            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            //.AddCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Login/");
            //    options.LoginPath = new PathString("/Login/");
            //})
            .AddCookie(options =>
            {
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Login";
                options.AccessDeniedPath = "/Security/Login";

               // Remove the ReturnUrl GET parameter from the sign in page.
               options.Events = new CookieAuthenticationEvents()
               {
                   OnRedirectToLogin = redirectContext =>
                   {
                       string redirectUri = redirectContext.RedirectUri;

                       UriHelper.FromAbsolute(
                           redirectUri,
                           out string scheme,
                           out HostString host,
                           out PathString path,
                           out QueryString query,
                           out FragmentString fragment);

                       redirectUri = UriHelper.BuildAbsolute(scheme, host, path);

                       redirectContext.Response.Redirect(redirectUri);

                       return Task.CompletedTask;
                   }
               };
            })

            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecreteforauth")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(60)
                };
            });

            

            // For refreshing view pages
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddDataProtection();

           services.AddMvc(config =>
            {
                // Require a authenticated user
                //var policy = new AuthorizationPolicyBuilder()
                //   .RequireAuthenticatedUser()
                //   .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.EnableEndpointRouting = false;

            });

            //services.AddControllersWithViews(config =>
            //{
                
               
            //});

            services.AddSpaStaticFiles(configuration =>
            {
                //development
                configuration.RootPath = "ClientApp";
                // publish
                 configuration.RootPath = "ClientApp/dist";
            });

            //services.AddControllers(opt =>
            //    opt.Filters.Add(new AuthorizeFilter(policy))
            //    );


       

           // services.ConfigureApplicationCookie(options => options.LoginPath = "/LogIn");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationBuilder app2, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });




            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSpaStaticFiles();




            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            app.Use(next => context =>
            {
            //if (context.Request.Path == "/")
            //{
                // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
            //}

                return next(context);
            });

            //app.Use(next => context =>
            //{
            //    string path = context.Request.Path.Value;
            //    if (path.IndexOf("a", StringComparison.OrdinalIgnoreCase) != -1 || path.IndexOf("b", StringComparison.OrdinalIgnoreCase) != -1)
            //    {
            //        // The request token can be sent as a JavaScript-readable cookie,
            //        // and Angular uses it by default.
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
            //    }
            //    return next(context);
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.Use(async (context, next) =>
            {
                
                if (!context.User.Identity.IsAuthenticated)
                {
                    await context.ChallengeAsync();
                }
                else
                {
                    await next();
                }
            });

            app.Map("",
               adminApp =>
               {
                   app.UseSpa(spa =>
                   {
                       if (env.IsDevelopment())
                           spa.Options.SourcePath = "ClientApp";
                       else
                           spa.Options.SourcePath = "dist";

                       if (env.IsDevelopment())
                       {
                           spa.UseVueCli(npmScript: "serve",port:5002);
                       }

                   });

                   //adminApp.UseSpa(spa =>
                   //{
                   //    spa.Options.SourcePath = "ClientApp";
                   //    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                   //    {
                   //        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp"))
                   //    };
                   //    spa.UseVueCli(npmScript: "serve");
                   //});
               }
           );


        }
    }
}
