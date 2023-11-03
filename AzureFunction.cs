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
  
    [Function("CheckIfPrimeNumberOrNot")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" }, Summary = "Summary", Description = "Description")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NumberData), Required = true, Description = "The request")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var numberData = await req.ReadFromJsonAsync<NumberData>();
        _logger.LogInformation($"C# HTTP trigger function processed a request. Number: {numberData.Number}");

        if (numberData is not { Number: > 0 })
        {
            var data = req.CreateResponse(HttpStatusCode.BadRequest);
            data.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await data.WriteStringAsync("Please pass a number in the request body");
            return data;
        }
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync(PrimeNumber.IsPrime(numberData.Number) ? "true" : "false");
        return response;
    }

    private static class PrimeNumber
    {
        // A method that returns true if n is prime, false otherwise
        public static bool IsPrime(int n)
        {
            switch (n)
            {
                // If n is less than 2, it is not prime
                case < 2:
                    return false;
                // If n is 2, it is prime
                case 2:
                    return true;
            }

            // If n is even, it is not prime
            if (n % 2 == 0)
            {
                return false;
            }

            // Get the square root of n
            int sqrt = (int)Math.Sqrt(n);

            // Loop through the odd numbers from 3 to the square root of n
            for (int i = 3; i <= sqrt; i += 2)
            {
                // If i divides n evenly, n is not prime
                if (n % i == 0)
                {
                    return false;
                }
            }

            // If none of the above conditions are met, n is prime
            return true;
        }
    }
    
    public class NumberData
    {
        public int Number { get; set; }
    }
}