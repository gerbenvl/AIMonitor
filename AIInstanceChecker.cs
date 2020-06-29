using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace AIMonitor
{
    public class AIInstanceChecker: IAIInstanceChecker
    {

        public void CheckAiInstances(AIInstanceSetting[] aiInstancesToCheck, ILogger log, ExecutionContext context)
        {
            foreach (var aiInstanceToMonitor in aiInstancesToCheck)
            {
                CheckAiInstance(aiInstanceToMonitor.ApplicationId, aiInstanceToMonitor.ApiKey, aiInstanceToMonitor.Name, log, context);
            }
        }

        private void CheckAiInstance(string aiApplicationID, string aiApiKey, string applicationName, ILogger log, ExecutionContext context)
        {
            // PT5M = laatste 5 minuten, voor de zekerheid
            string applicationInsightsEndpoint = $"https://api.applicationinsights.io/v1/apps/{aiApplicationID}/metrics/exceptions/count?timespan=PT3M";

            var applicationInsightsHttpClient = new HttpClient();
            applicationInsightsHttpClient.DefaultRequestHeaders.Add("x-api-key", aiApiKey);
            var httpResponseMessage = applicationInsightsHttpClient.GetAsync(applicationInsightsEndpoint).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var aiResponse = AiResponse.FromJson(httpResponseMessage.Content.ReadAsStringAsync().Result);

                if (aiResponse.Value.ExceptionsCount.Sum > 0)
                {
                    log.LogInformation($"Fouten gevonden in {applicationName}");
                    FirebaseMessage.SendAsync(applicationName, "Er is wat stuk, ga kijken!", context);
                }
            }
            else
            {
                const string error = "De monitor function kan niet bij de API";
                throw new Exception(error);
            }
        }
    }
}
