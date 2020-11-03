using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class OData
    {
        [FunctionName("OData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            HttpClient newClient = new HttpClient();
            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, string.Format("https://services.odata.org/TripPinRESTierService/(S(vdwa3oul3hobwwlml331a0kl))/People"));

            HttpResponseMessage response = await newClient.SendAsync(newRequest);
            string jsonString = await response.Content.ReadAsStringAsync();
            ResponseODATA responseODATA = JsonConvert.DeserializeObject<ResponseODATA>(jsonString);

            return new OkObjectResult(responseODATA.value);
        }
    }

    #nullable enable
    public class City
    {
        public string? Name { get; set; }
        public string? CountryRegion { get; set; }
        public string? Region { get; set; }
    }

    public class AddressInfo
    {
        public string? Address { get; set; }
        public City? City { get; set; }
    }

    public class HomeAddress
    {
        public string? Address { get; set; }
        public City? City { get; set; }
    }

    public class User
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public List<string>? Emails { get; set; }
        public string? FavoriteFeature { get; set; }
        public List<string>? Features { get; set; }
        public int? Age { get; set; }
        public string? MiddleName { get; set; }
        public List<AddressInfo>? AddressInfo { get; set; }
        public HomeAddress? HomeAddress { get; set; }
    }

    public class ResponseODATA
    {
        [JsonProperty("@odata.context")]
        public string? OdataContext { get; set; }
        public List<User>? value { get; set; }
    }

}