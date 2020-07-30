using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Infrastructure.Data;


namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        /*
            a) When this startup calls is initialised the configurations are Injected
                into this the Constructor of the startup class
            b) This IConfiguration represents a KeyValue configuration properties, as like in the appsettings.json
            c) We can access those individual keys in the appsettings.json via this Iconfiguration injected
             into the startup's constructor.
        */
        public Startup(IConfiguration config)
        {
            // nothing bu the configuration from the appsettings.json file
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        /*
         Often refered to as dependancy injection container,
         any services we want to be availble for our application has to be added here
        */
        public void ConfigureServices(IServiceCollection services)
        {
            /*
                This service adds controller support for our application
            */
            services.AddControllers();

            /*
                Configure the connections string to sqlite from the appsettings.development.json
            */
            services.AddDbContext<StoreContext>(x =>
                 x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /* This is responsible for redirecting normal http request to https request.
            Thats why we see two requests inthe chrome->developer-tools->network tab
            one is the sent http request, which is then respoded with temporary redirection
            which the generates the next request to the https
            thats why the browser displays the security alert even when we try copnnect the http://localhost:5000
            */
            app.UseHttpsRedirection();

            /*
            Routing the Urls to the endpoints
            */
            app.UseRouting();

            /*
                For authentication & Authorization
            */
            app.UseAuthorization();

            /*
            On Application start this Maps the controllers to the endpoints
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
