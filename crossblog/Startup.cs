using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Exceptions;
using crossblog.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace crossblog
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
            //logs
            services.AddLogging();
            
            //cors
            services.AddCors();

            //regiter db context
            ConfigureDatabase(services);

            //core features 
            ConfigureRepositories(services);

            // enable controller 
            services.AddMvc().AddJsonOptions(options =>
            {
                var settings = options.SerializerSettings;

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                settings.Formatting = Formatting.Indented;
                settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<CrossBlogDbContext>(options => options.UseMySql(Configuration.GetConnectionString("CrossBlog")))
                .AddScoped<BlogDbInitializer>();
        }

        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IArticleRepository, ArticleRepository>()
                .AddTransient<ICommentRepository, CommentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            env.EnvironmentName = EnvironmentName.Production;
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<ErrorWrappingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/home/error/{0}");
                app.UseExceptionHandler("/home/error/500");
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
