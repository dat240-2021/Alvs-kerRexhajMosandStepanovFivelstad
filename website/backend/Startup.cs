using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Authentication;
using Domain.Authentication.Services;
using backend.Core.Domain.Games;
using backend.Core.Domain.Images.Utils;
using backend.Core.Domain.Lobby.Services;
using Microsoft.AspNetCore.Http;

namespace backend
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
            services.AddDbContext<GameContext>(options =>
            {
                // The folders specified here must exist, otherwise the database is never created
                // It does not create the folder, nor does it return an error.
                options.UseSqlite($"Data Source={Path.Combine("Infrastructure", "Data", "game.db")}");
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<ILobbyService, LobbyService>();
            services.AddSingleton<IGameService, GameService>();

            services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<GameContext>()
            .AddUserManager<UserManager<User>>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });


            services.AddMediatR(typeof(Startup));

            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GameContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize db with data if necessary.
            if (!db.Images.Any())
            {
                var ImagePreprocessor = new ImagePreprocessor();
                ImagePreprocessor.Parse();
                db.ImageCategories.AddRange(ImagePreprocessor.Categories);
                db.Images.AddRange(ImagePreprocessor.Images);
                db.SaveChanges(); // Save before mapping
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LobbyHub>("/hub/games");
                endpoints.MapHub<GameHub>("/hub/game");
            });
        }
    }
}
