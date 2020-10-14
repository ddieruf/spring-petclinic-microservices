using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Connector.SqlServer.EFCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Management.Tracing;
using spring_petclinic_customers_api.Data;

namespace spring_petclinic_customers_api
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      Environment = env;
    }

    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //DATA CONTEXTS
			if (Environment.IsDevelopment()) {
				services.AddDbContext<CustomersContext>(options => options.UseInMemoryDatabase("PetClinic_Customers"));
			}else{
				services.AddDbContext<CustomersContext>(options => options.UseSqlServer(Configuration));
			}

      //REPOSITORIES
      services.AddScoped<Repository.IPets, Repository.Pets>();
      services.AddScoped<Repository.IOwners, Repository.Owners>();

      services.AddControllers();

      services.AddDistributedTracing(Configuration, builder => builder.UseZipkinWithTraceOptions(services));

      services.AddSwaggerGen();

      services.AddAuthorization();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, CustomersContext dbContext)
    {
      if (env.IsDevelopment())
      {
        logger.LogInformation("Running as development environment");
        app.UseDeveloperExceptionPage();

        dbContext.SeedAll();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Clinic Customers Service");
      });

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
