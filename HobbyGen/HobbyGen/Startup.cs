﻿namespace HobbyGen
{
    using HobbyGen.Controllers;
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// Application startup and configuration class for the MVC API web-end
    /// </summary>
    public class Startup
    {
        private const string DB_LOC = "hobby";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration">The configuration to use for the application</param>
        [Obsolete("This class is automated, please do not create instances of this class")]
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration attached to the application
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Support Razor pages
            services.AddMvc();

            // Add singleton for dummy data
            services.AddSingleton<DummyController, DummyController>();

            // TODO: Move this memory db to testing later
            // eg https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
            services.AddDbContext<GeneralContext>(
                opt =>
                opt
                .UseInMemoryDatabase(DB_LOC));

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="env">The hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable razor pages
            app.UseMvc();

            // For the wwwroot folder
            app.UseStaticFiles(); 
        }
    }
}
