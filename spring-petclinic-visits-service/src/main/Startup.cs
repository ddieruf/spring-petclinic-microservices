using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Connector.SqlServer.EFCore;
using Microsoft.Extensions.Logging;
using Steeltoe.Management.Tracing;
using spring_petclinic_visits_api.Infrastructure;
using spring_petclinic_visits_api.Infrastructure.Repository;

namespace spring_petclinic_visits_api
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
      //DATA CONTEXT
      services.AddDbContext<VisitsContext>(options => options.UseInMemoryDatabase("PetClinic_Customers"));

      //REPOSITORIES
      services.AddScoped<IVisits, Visits>();

      services.AddControllers();

      services.AddDistributedTracing(Configuration, builder => builder.UseZipkinWithTraceOptions(services));

      services.AddSwaggerGen();

      services.AddAuthorization();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, VisitsContext dbContext)
    {
      switch (Environment.EnvironmentName) {
        case ("Development"):
        case ("Docker"):
          logger.LogInformation($"Running as {Environment.EnvironmentName} environment");
          app.UseDeveloperExceptionPage();
          break;
        default:
          break;
      };

      dbContext.SeedAll();

      //app.UseHttpsRedirection();

      app.UseRouting();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Clinic Visits Service");
      });

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
