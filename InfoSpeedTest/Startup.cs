using Common.Service.Provider.Interfaces;
using Data.Service.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace InfoSpeedTest
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
            services.AddControllers();
            services.AddSingleton<IDataManager, DataManager>();
            services.AddSingleton<IDataStore, DataStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var dataManager = app.ApplicationServices.GetService(typeof(IDataManager)) as IDataManager;

            lifetime.ApplicationStarted.Register(OnApplicationStartedAsync(dataManager).Wait);
        }

        private async Task<Action> OnApplicationStartedAsync(IDataManager dataManager)
        {
            var filepaths = Configuration.GetSection("Files:paths").Get<string[]>();
            var folderPath = System.IO.Directory.GetCurrentDirectory();

            for (int i = 0; i < filepaths.Length ; i++)
            {
                filepaths[i] = Path.Combine(folderPath,"Data",filepaths[i]);
            }

            await Task.Run(() =>
            {
                dataManager.LoadLogDataFromFile(filepaths.ToArray());
            }).ConfigureAwait(false);

            return null;

        }
    }
}
