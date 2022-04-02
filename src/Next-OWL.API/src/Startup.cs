using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Next_OWL.Models.Config;
using Next_OWL.Service;

namespace Next_OWL
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
            services.AddCors();
            services.AddHealthChecks();

            var owlAPIConfig = new OWLApiConfig();
            Configuration.Bind("OWLApi", owlAPIConfig);

            services.AddSingleton(owlAPIConfig);
            services.AddScoped<IOwlService, OwlService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .WithMethods("GET", "OPTIONS")
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/api/health");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
