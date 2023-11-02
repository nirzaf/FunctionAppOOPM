using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace FunctionAppOOPM;

public class AzureFunction
{
    private readonly ILogger _logger;

    public AzureFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AzureFunction>();
    }

    [Function("AzureFunction")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" }, Summary = "Summary", Description = "Description")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}