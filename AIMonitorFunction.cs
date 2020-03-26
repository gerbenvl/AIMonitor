using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AIMonitor
{
    public static class AIMonitorFunction
    {
        [FunctionName("AIMonitorFunction")]
        public static void Run([TimerTrigger("0 */3 * * * *")]TimerInfo time, ILogger log)
        {
            string aiInstancesToMonitorSetting = Environment.GetEnvironmentVariable("AiInstancesToMonitor");
            var aiInstancesToMonitor = JsonConvert.DeserializeObject<List<AIInstanceSetting>>(aiInstancesToMonitorSetting);

            foreach (var aiInstanceToMonitor in aiInstancesToMonitor)
            {
                CheckAiInstance(aiInstanceToMonitor.ApplicationId, aiInstanceToMonitor.ApiKey, aiInstanceToMonitor.Name, log);
            }
        }

        private static void CheckAiInstance(string aiApplicationID, string aiApiKey, string applicationName, ILogger log)
        {
            // PT5M = laatste 5 minuten, voor de zekerheid
            string applicationInsightsEndpoint = $"https://api.applicationinsights.io/v1/apps/{aiApplicationID}/metrics/exceptions/count?timespan=PT5M";

            var applicationInsightsHttpClient = new HttpClient();
            applicationInsightsHttpClient.DefaultRequestHeaders.Add("x-api-key", aiApiKey);
            var httpResponseMessage = applicationInsightsHttpClient.GetAsync(applicationInsightsEndpoint).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var aiResponse = AiResponse.FromJson(httpResponseMessage.Content.ReadAsStringAsync().Result);

                if (aiResponse.Value.ExceptionsCount.Sum > 0)
                {
                    log.LogInformation($"Fouten gevonden in {applicationName}");
                    FirebaseMessage.SendAsync(applicationName, "Er is wat stuk, ga kijken!");                    
                }
                else
                {
                    log.LogInformation($"{applicationName} geen fouten gevonden");
                }
            }
            else
            {
                const string error = "De monitor function kan niet bij de API";
                log.LogInformation(error);
                throw new Exception(error);
            }
        }
    }
}
