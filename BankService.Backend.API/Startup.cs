using BankService.Backend.BusinessLogic.Handler;
using BankService.Backend.Persistance;
using BankService.Backend.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankService.Backend.API
{
    public class Startup
    {
        private const bool UseInMemoryDb = true; // In memory or Postgres DB

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            if (UseInMemoryDb)
            {
                services.AddDbContext<InMemoryDbContext>(options => options.UseInMemoryDatabase(databaseName: "BankService"));
                services.AddTransient<BankServiceDbContext, InMemoryDbContext>();
            }
            else
            {
                services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Postgres"), options => options.MigrationsAssembly("BankServiceBackend")));
                services.AddTransient<BankServiceDbContext, PostgresDbContext>();
            }

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransactionHandler, TransactionHandler>();
            services.AddTransient<IAccountAssignmentHandler, AccountAssignmentHandler>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankService API v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable HTTPs
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
