using System.Collections.Generic;
using System.Net.Http;

namespace AIMonitor
{
    public class AIInstanceChecker : IAIInstanceChecker
    {
        public IEnumerable<AppStatus> CheckAiInstances(AIInstanceSetting[] aiInstancesToCheck)
        {
            var appStatusus = new List<AppStatus>();
            
            foreach (var aiInstanceToMonitor in aiInstancesToCheck)
            {
                bool hasExceptions = DoesAiInstanceContainExceptions(aiInstanceToMonitor.ApplicationId, aiInstanceToMonitor.ApiKey);
                var appStatus = new AppStatus()
                {
                    Name = aiInstanceToMonitor.Name,
                    ContainsExceptions = hasExceptions
                };
                appStatusus.Add(appStatus);
            }

            return appStatusus;
        }

        private bool DoesAiInstanceContainExceptions(string aiApplicationID, string aiApiKey)
        {
            // PT5M = laatste 5 minuten, voor de zekerheid
            string applicationInsightsEndpoint = $"https://api.applicationinsights.io/v1/apps/{aiApplicationID}/metrics/exceptions/count?timespan=PT5M";

            var applicationInsightsHttpClient = new HttpClient();
            applicationInsightsHttpClient.DefaultRequestHeaders.Add("x-api-key", aiApiKey);
            var httpResponseMessage = applicationInsightsHttpClient.GetAsync(applicationInsightsEndpoint).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            var aiResponse = AiResponse.FromJson(httpResponseMessage.Content.ReadAsStringAsync().Result);
            if (aiResponse.Value.ExceptionsCount.Sum > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
