using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DemoFunc
{
    //Função para realizar requisição
    public static class OnDataViaLinhaDeComando
    {
        [FunctionName("OnDataViaLinhaDeComando")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# Trigger iniciada");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (string.IsNullOrEmpty(name)) 
            {
                return new BadRequestObjectResult("Parâmetro name não encontrado");
            }
           

            return new OkObjectResult($"Olá {name}, como vai?");
        }
    }
}
