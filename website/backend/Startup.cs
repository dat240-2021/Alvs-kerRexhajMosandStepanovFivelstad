using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame;
using backend.Core.Domain.BackendGame.Services;
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
                //The folders specified here must exist, otherwise the database is never created
                //It does not create the folder, nor does it return an error.
                options.UseSqlite($"Data Source={Path.Combine("Infrastructure","Data", "game.db")}");
            });

            services.AddScoped<IAuthenticationService,AuthenticationService>();
            services.AddSingleton<IBackendGameService, BackendGameService>();



            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<User,IdentityRole<Guid>>()
            .AddEntityFrameworkStores<GameContext>()
            .AddUserManager<UserManager<User>>();
            

            // services.AddAuthentication().AddIdentityServerJwt();

            services.AddMediatR(typeof(Startup));

            services.AddControllers();
            services.AddSignalR();
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,GameContext db)
        {
            // db.SaveChanges();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            //Needed for login
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();



            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GamesHub>("/hub/games");
            });
        }
    }
}
