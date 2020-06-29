using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AIMonitor
{
    public class AIMonitorFunction
    {
        private readonly IAIInstanceChecker _aiInstanceChecker;

        public AIMonitorFunction(IAIInstanceChecker aiInstanceChecker)
        {
            _aiInstanceChecker = aiInstanceChecker;
        }

        [FunctionName("AIMonitorFunction")]
        public void Run([TimerTrigger("0 */2 * * * *")]TimerInfo time, ILogger log, ExecutionContext context)
        {
            string aiInstancesToMonitorSetting = Environment.GetEnvironmentVariable("AiInstancesToMonitor");
            var aiInstancesToMonitor = JsonConvert.DeserializeObject<List<AIInstanceSetting>>(aiInstancesToMonitorSetting).ToArray();
            _aiInstanceChecker.CheckAiInstances(aiInstancesToMonitor, log, context);
        }
    }
}
