using crossblog.Domain;
using crossblog.tests.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace crossblog.tests
{
    public class Startup : crossblog.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            // Replace default database connection with In-Memory database
            services.AddDbContext<CrossBlogDbContext>(options => options.UseInMemoryDatabase("cross-blog_test_db"));

            // Register the database seeder
            services.AddTransient<DatabaseSeeder>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Perform all the configuration in the base class
            base.Configure(app, env, loggerFactory);

            // Now seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();

                seeder.Seed();
            }
        }

    }
}
