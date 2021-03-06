using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogFunctionApp
{
    public static class LogFunction
    {
        [FunctionName("LogFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // http://localhost:7071/api/LogFunction?logData=hi
            // https://logfunctionapp20200311020048.azurewebsites.net/api/LogFunction?logData=poo
            // https://LogFuncrs.azurewebsites.net/api/LogFuncthis?logData=poo
            log.LogInformation("C# HTTP trigger function processed a request.");

            string logData = req.Query["logData"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            logData = logData ?? data?.logData;

            string responseMessage = string.IsNullOrEmpty(logData)
                ? "HTTP VSCODE function executed successfully. Pass a logData in the query string or in the request body for a personalized response."
                : $"Log Data: {logData}. HTTP VSCODE logging OK.";

            return new OkObjectResult(responseMessage);
        }

    }
}
