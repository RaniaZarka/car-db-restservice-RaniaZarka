using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;


namespace WebApiCar
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
            services.AddCors(options =>
            {
                // 1 st policy --> allow specific origin
                options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("https://azure.microsoft.com/").AllowAnyHeader().AllowAnyMethod();
                });

                // 2nd Policy --> allow any origins -- Now you have to public your services
                options.AddPolicy("AllowAnyOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });

                // 3rd Policy --> allow only Get Put Method 
                options.AddPolicy("AllowAnyOriginsGetPUT",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET", "POST");
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Web Car API",
                    Version = "v1",
                    Description = "A simåle web API med basic CRUD operations",
                    Contact = new Contact
                    {
                        Name = "Martin Kierkegaard",
                        Email = string.Empty,
                        Url =   string.Empty
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // You must have to follow the exact sequence to implement the CORS....
            app.UseHttpsRedirection();
            // 1st use Use.Routing()
            app.UseRouting();
            // 2nd use os UseCors
            // Shows which cors policy you would to like to define here. use.cors
            app.UseCors("AllowAnyOrigins");
            //app.UseCors("AllowSpecificOrigins");
            //app.UseCors("AllowAnyOrigins");
            // 3rd use UseAuthorization
            app.UseAuthorization();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Car");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
