using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;
// using AdventureWorkContext;
using Devart.Data.Linq;

namespace FunctionApp1
{
    public static class salesLT
    {
        [FunctionName("salesLT")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            //AdventureWorkDataContext dblib = new AdventureWorkDataContext();
            //var products = dblib.Products;
            string color = req.Query["color"];

            // log.LogInformation(products.ToString());

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            color = color ?? data?.color;

            string responseMessage = string.IsNullOrEmpty(color)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {color}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
