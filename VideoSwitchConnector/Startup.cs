using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BmdSwitcher;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

namespace VideoSwitchConnector
{
    public class Startup
    {
        private Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSimpleInjector(_container, options =>
            {
                options.AddAspNetCore()
                .AddControllerActivation();
            });

            var switcher = new BmdSwitcher.BmdSwitcher();
            switcher.Connect("192.168.0.137");
            _container.Register<Switcher>(() => switcher, Lifestyle.Singleton);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSimpleInjector(_container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _container.Verify();
        }
    }
}
