using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AIMonitor
{
    public interface IAIInstanceChecker
    {
        void CheckAiInstances(AIInstanceSetting[] aiInstancesToCheck, ILogger log, ExecutionContext context);
    }
}
