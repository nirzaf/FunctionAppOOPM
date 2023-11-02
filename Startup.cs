using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
public class Startup : IWebJobsStartup
{ 
    public void Configure(IWebJobsBuilder builder)
    {
        builder.Services.AddLogging();
    }
}